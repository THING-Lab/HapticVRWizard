using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveStrokeReader : MonoBehaviour {
    private bool _isButtonHeld = false;
    private bool _isTouchHeld = false;
    private Vector2 _prevTouch = new Vector2(0, 0);
    public float _moveThreshold = 0.005f;
    private float _radiusScale = 1.0f;
    
    // Hack to allow for a new position on first
    private Vector3 _lastPos = new Vector3(-1000, -1000, -1000);
    public GameObject _cursor;
    public GameObject _toolManager;

    private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

    void Awake () {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {
        ITool currentTool = _toolManager.GetComponent<ToolManager>().CurrentTool;

        if (Controller.GetHairTriggerDown()) {
            _isButtonHeld = true;
            currentTool.StartStroke();
        }

        if (Controller.GetHairTriggerUp()) {
            _isButtonHeld = false;
            currentTool.EndStroke();
        }

        if (_isButtonHeld) {
            Vector3 currentPos = _trackedObj.transform.position;

            // We might need to add more sophisticated position smoothing than this
            if (Vector3.Distance(currentPos, _lastPos) >= _moveThreshold)
            {
                currentTool.UpdateStroke(_cursor.transform.position, _radiusScale);
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
                _radiusScale += dx;
            }

            _prevTouch = axis;
        } else if (currentTouch) {
            _prevTouch = Controller.GetAxis();
        }

        _isTouchHeld = currentTouch; 
    }
}
