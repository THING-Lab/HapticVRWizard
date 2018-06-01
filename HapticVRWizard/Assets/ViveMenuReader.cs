using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveMenuReader : MonoBehaviour {
	private bool _isPadHeld = false;
	private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

	// Use this for initialization
	void Awake () {
		_trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
			Vector2 axis = Controller.GetAxis();

			if (axis.x > 0.2f) {
				Debug.Log("Redo!");
			} else if (axis.x < -0.2f) {
				Debug.Log("Undo!");
			}
		}
	}
}
