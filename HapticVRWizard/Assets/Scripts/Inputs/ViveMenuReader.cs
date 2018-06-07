using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveMenuReader : MonoBehaviour {
	private bool _isTouchHeld = false;
	private float _menuRotation = 0f;
	private Vector2 _prevTouch = new Vector2(0, 0);
	private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

	public ToolManager _toolManager;
	public Transform _menuTransform;

	// Use this for initialization
	void Awake () {
		_trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector2 axis = Controller.GetAxis();

			if (axis.x > 0.05f) {
				_toolManager.Redo();
			} else if (axis.x < -0.05f) {
				_toolManager.Undo();
			}
		}

		// Menu Rotation
        bool currentTouch = Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad);
        if (currentTouch && _isTouchHeld) {
            Vector2 axis = Controller.GetAxis();
            float dx = axis.x - _prevTouch.x;
			// Scale dx so rotation is more comfortable
			_menuRotation -= dx * 30;
            
            // Use threshold
            if (dx > 0.0001 || dx < -0.0001) {
                _menuTransform.localRotation = Quaternion.AngleAxis(_menuRotation, Vector3.forward);
            }

            _prevTouch = axis;
        } else if (currentTouch) {
            _prevTouch = Controller.GetAxis();
        }

        _isTouchHeld = currentTouch; 
	}
}
