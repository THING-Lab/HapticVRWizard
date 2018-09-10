using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveSketchCameraController : MonoBehaviour {
	public GameObject _sketchCamera;
	private bool _isTriggerHeld;

	private SteamVR_TrackedObject _trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

	void Awake () {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Controller.GetHairTriggerDown()) {
            _isTriggerHeld = true;
			// _sketchCamera.transform.position = transform.position;
        }

        if (Controller.GetHairTriggerUp()) {
            _isTriggerHeld = false;
        }

		if (_isTriggerHeld) {
			// _sketchCamera.transform.LookAt(transform.position);
			_sketchCamera.transform.position = transform.position;
			_sketchCamera.transform.rotation = transform.rotation;
		}
	}
}
