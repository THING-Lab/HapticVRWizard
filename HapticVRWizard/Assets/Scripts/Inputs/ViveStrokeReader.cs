﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class ViveStrokeReader : MonoBehaviour {
    private bool _isTriggerHeld = false;
    private bool _isTouchHeld = false;
    private Vector2 _prevTouch = new Vector2(0, 0);
    public float _moveThreshold = 0.005f;

    private float _pressTime = 0f;
    private const float LONG_PRESS_TIME = 1.2f;

    private float _radiusScale = 1.0f;
    private float _defaultScale = 0.02f;
    private float _currentRadius = 0.02f;

    // Hack to allow for a new position on first
    private Vector3 _lastPos = new Vector3(-1000, -1000, -1000);
    public GameObject _cursor;
    public ToolManager _toolManager;
    private int _drawParentId;

    public Transform _defaultDrawParent;
    private Transform _drawParent;
    public Transform DrawParent {
        get { return _drawParent; }
        set { _drawParent = value; }
    }

    private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    private bool _isPointerMode = false;
    public bool IsPointerMode {
        get { return _isPointerMode; }
    }

    private bool _isSelectorMode = false;
    public bool IsSelectorMode {
        get { return _isSelectorMode; }
        set { _isSelectorMode = value; }
    }

    private bool CanDraw {
        get { return (!_isPointerMode && !_isSelectorMode); }
    }

    // REPLACE THIS WITH PROPERTY
    public void SetPointerMode(bool setting) {
        _isPointerMode = setting;
    }

    void Awake () {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
        _drawParent = _defaultDrawParent;
    }

    // Update is called once per frame
    void Update () {
        // Might wanna do this less
        _cursor.SetActive(!(_isPointerMode || _isSelectorMode));

        // Only start drawing if not pointing at menu
        if (Controller.GetHairTriggerDown() && CanDraw) {
            _isTriggerHeld = true;
            _toolManager.StartStroke(DrawParent);
            _pressTime = 0;
        }

        if (Controller.GetHairTriggerUp() && _isTriggerHeld) {
            _isTriggerHeld = false;
            _toolManager.EndStroke(DrawParent);
            
            if (!_toolManager.IsStrokeTool && _pressTime > LONG_PRESS_TIME) {
                _toolManager.EndTriangle(DrawParent);
            }
        }

        if (_isTriggerHeld || !_toolManager.IsStrokeTool) {
            // Make cursor point relative to current parent (Trackers / World Drawing)
            Vector3 currentPos = DrawParent.InverseTransformPoint(_cursor.transform.position);

            // We might need to add more sophisticated position smoothing than this
            if (Vector3.Distance(currentPos, _lastPos) >= _moveThreshold)
            {
                _toolManager.UpdateStroke(currentPos, transform.rotation, _currentRadius);
                _lastPos = currentPos;
            }
        }

        if (_isTriggerHeld) {
            _pressTime += Time.deltaTime;
            if (!_toolManager.IsStrokeTool && _pressTime > LONG_PRESS_TIME) {
                Controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_SteamVR_Trigger);
            }
        }

        // Radius Change
        bool currentTouch = Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad);
        if (currentTouch && _isTouchHeld) {
            Vector2 axis = Controller.GetAxis();
            float dx = axis.x - _prevTouch.x;
            
            // Use threshold
            if (dx > 0.0001 || dx < -0.0001) {
                float newScale = _radiusScale + dx;

                // Clamp scale so it doesn't break
                if (newScale < 6.5f && newScale > 0.15f) {
                    _radiusScale = newScale;
                    _currentRadius = _radiusScale * _defaultScale;

                    // Scale the cursor
                    float s = (_currentRadius) - _cursor.transform.localScale.x;
                    _cursor.transform.localScale += new Vector3(s,s,s);
                }
            }

            _prevTouch = axis;
        } else if (currentTouch) {
            _prevTouch = Controller.GetAxis();
        }

        _isTouchHeld = currentTouch;
    }
}
