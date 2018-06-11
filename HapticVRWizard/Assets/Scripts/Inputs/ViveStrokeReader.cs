using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViveStrokeReader : MonoBehaviour {
    private bool _isTriggerHeld = false;
    private bool _isTouchHeld = false;
    private Vector2 _prevTouch = new Vector2(0, 0);
    public float _moveThreshold = 0.005f;

    private float _radiusScale = 1.0f;
    private float _defaultScale = 0.02f;
    private float _currentRadius = 0.02f;

    // Hack to allow for a new position on first
    private Vector3 _lastPos = new Vector3(-1000, -1000, -1000);
    public GameObject _cursor;
    public ToolManager _toolManager;
    private int _drawParentId;
    public List<Transform> _trackers;
    private Transform DrawParent {
        get { return _trackers[_drawParentId]; }
    }

    private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    private bool _isPointerMode = false;
    public bool IsPointerMode {
        get { return _isPointerMode; }
    }

    public void SetPointerMode(bool setting) {
        _isPointerMode = setting;
        _cursor.SetActive(!setting);
    }

    public void SetDrawParent(int newId) {
        _drawParentId = newId;
    }

    void Awake () {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {
        // Only start drawing if not pointing at menu
        if (Controller.GetHairTriggerDown() && !_isPointerMode) {
            _isTriggerHeld = true;
            _toolManager.StartStroke(DrawParent);
        }

        if (Controller.GetHairTriggerUp() && _isTriggerHeld) {
            _isTriggerHeld = false;
            _toolManager.EndStroke(DrawParent);
        }

        if (_isTriggerHeld) {
            // Make cursor point relative to current parent (Trackers / World Drawing)
            Vector3 currentPos = DrawParent.InverseTransformPoint(_cursor.transform.position);

            // We might need to add more sophisticated position smoothing than this
            if (Vector3.Distance(currentPos, _lastPos) >= _moveThreshold)
            {
                _toolManager.UpdateStroke(currentPos, _currentRadius);
                _lastPos = currentPos;
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

        // Debug Keyboard stuff for trackers
        if(Input.GetKeyDown(KeyCode.Alpha0)) {
            _drawParentId = 0;
		}

        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            _drawParentId = 1;
		}
    }
}
