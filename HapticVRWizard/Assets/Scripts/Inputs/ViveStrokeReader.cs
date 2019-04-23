using System;
using UnityEngine;
using Valve.VR;


public class ViveStrokeReader : MonoBehaviour
{
    private bool _isTriggerHeld = false;
    private bool _areBristlesDown = false;
    private bool _isTouchHeld = false;
    private Vector2 _prevTouch = new Vector2(0, 0);
    public float _moveThreshold = 0.005f;

    private float _pressTime = 0f;
    private const float LONG_PRESS_TIME = 1.2f;

    private float _radiusScale = 0.15f;
    private float _defaultScale = 0.02f;
    private float _currentRadius = 0.02f;

    // Hack to allow for a new position on first
    private Vector3 _lastPos = new Vector3(-1000, -1000, -1000);
    public GameObject _cursor;
    public GameObject _model;
    public ToolManager _toolManager;
    private int _drawParentId;

    public Transform _defaultDrawParent;
    private Transform _drawParent;
    public Transform DrawParent
    {
        get { return _drawParent; }
        set { _drawParent = value; }
    }


    //Start Chandan Changes 
    //To read touchpad position values
    public SteamVR_Action_Vector2 TouchpadAction;

    //For TriggerHaptics
    public SteamVR_Action_Vibration haptics;
    //End Chandan Changes

    private SteamVR_TrackedObject _trackedObj;

    private bool _isPointerMode = false;

    public bool IsPointerMode
    {
        get { return _isPointerMode; }
    }

    private bool _isSelectorMode = false;
    public bool IsSelectorMode
    {
        get { return _isSelectorMode; }
        set { _isSelectorMode = value; }
    }

    private bool CanDraw
    {
        get { return (!_isPointerMode && !_isSelectorMode); }
    }

    public bool IsBrushTool
    {
        get { return (ToolManager.isBrushTool); }
    }

    // REPLACE THIS WITH PROPERTY
    public void SetPointerMode(bool setting)
    {
        _isPointerMode = setting;
    }

    void Awake()
    {
        try
        {
            _trackedObj = GetComponent<SteamVR_TrackedObject>();
            _drawParent = _defaultDrawParent;
        }

        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    // Update is called once per frame
    void Update()
    {

        try
        {
            // Might wanna do this less
            _cursor.SetActive(!(_isPointerMode || _isSelectorMode));
            _model.SetActive(_isPointerMode || _isSelectorMode);
            // Only start drawing if not pointing at menu
            if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand) && CanDraw && !IsBrushTool)
            {
                _isTriggerHeld = true;
                _toolManager.StartStroke(DrawParent);
                _pressTime = 0;
            }

            if (IsBrushTool && !SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand) && CanDraw)
            {
                if (BrushSensorPolling.IsBristleBent[0] || BrushSensorPolling.IsBristleBent[1] || BrushSensorPolling.IsBristleBent[2] ||
                    BrushSensorPolling.IsBristleBent[3] || BrushSensorPolling.IsBristleBent[4] || BrushSensorPolling.IsBristleBent[5])
                {
                    _isTriggerHeld = true;
                    _areBristlesDown = true;
                    _toolManager.StartStroke(DrawParent);
                    _pressTime = 0;
                }

                else if(_areBristlesDown)
                {
                    _isTriggerHeld = false;
                    _toolManager.EndStroke(DrawParent);
                    _areBristlesDown = false;
                }
            }

            if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand) && _isTriggerHeld)
            {
                _isTriggerHeld = false;
                _toolManager.EndStroke(DrawParent);

                if (!_toolManager.IsStrokeTool && _pressTime > LONG_PRESS_TIME)
                {
                    _toolManager.EndTriangle(DrawParent);
                }
            }

            if (_isTriggerHeld || !_toolManager.IsStrokeTool)
            {
                // Make cursor point relative to current parent (Trackers / World Drawing)
                Vector3 currentPos = DrawParent.InverseTransformPoint(_cursor.transform.position);

                // We might need to add more sophisticated position smoothing than this
                if (Vector3.Distance(currentPos, _lastPos) >= _moveThreshold)
                {
                    _toolManager.UpdateStroke(currentPos, transform.rotation, _currentRadius);
                    _lastPos = currentPos;
                }
            }

            if (_isTriggerHeld)
            {
                _pressTime += Time.deltaTime;
                if (!_toolManager.IsStrokeTool && _pressTime > LONG_PRESS_TIME)
                {
                    //Controller.TriggerHapticPulse(500, EVRButtonId.k_EButton_SteamVR_Trigger);
                    if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand))
                    {
                        Pulse(1, 50, 75, SteamVR_Input_Sources.RightHand);
                    }

                }
            }

            // Radius Change
            Vector2 touchpadValue = TouchpadAction.GetAxis(SteamVR_Input_Sources.RightHand);
            bool currentTouch = touchpadValue != Vector2.zero ? true : false;

            if (currentTouch && _isTouchHeld)
            {
                Vector2 axis = touchpadValue;
                float dx = axis.x - _prevTouch.x;

                // Use threshold
                if (dx > 0.0001 || dx < -0.0001)
                {
                    float newScale = _radiusScale + dx;

                    // Clamp scale so it doesn't break
                    if (newScale < 6.5f && newScale > 0.15f)
                    {
                        _radiusScale = newScale;
                        _currentRadius = _radiusScale * _defaultScale;

                        // Scale the cursor
                        float s = (_currentRadius) - _cursor.transform.localScale.x;
                        _cursor.transform.localScale += new Vector3(s, s, s);
                    }
                }

                _prevTouch = axis;
            }
            else if (currentTouch)
            {
                _prevTouch = touchpadValue;
            }

            _isTouchHeld = currentTouch;
        }

        catch (Exception e)
        {
            Debug.Log("!!!!!!!Exception occured!!!!!!!");
            Debug.LogException(e, this);
        }
    }

    public void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        haptics.Execute(0, duration, frequency, amplitude, source); //0(First Parameter): duration after which haptics should start.
        //print("Pulse from :------>" + source.ToString());
    }
}
