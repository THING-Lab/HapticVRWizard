using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveSketchCameraController : MonoBehaviour {
	public GameObject _sketchCamera;
	private bool _isTriggerHeld;

	private SteamVR_TrackedObject _trackedObj;

    void Awake()
    {
        try
        {
            _trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        catch (Exception e)
        {
            Debug.Log("!!!!!!!Exception occured!!!!!!!");
            Debug.LogException(e, this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            //Trigger press
            if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
            {
                _isTriggerHeld = true;
                // _sketchCamera.transform.position = transform.position;
            }

            //Trigger depress
            if (SteamVR_Actions._default.GrabPinch.GetStateUp(SteamVR_Input_Sources.Any))
            {
                _isTriggerHeld = false;
            }

            if (_isTriggerHeld)
            {
                // _sketchCamera.transform.LookAt(transform.position);
                _sketchCamera.transform.position = transform.position;
                _sketchCamera.transform.rotation = transform.rotation;
            }
        }

        catch (Exception e)
        {
            Debug.Log("!!!!!!!Exception occured!!!!!!!");
            Debug.LogException(e, this);
        }
    }
}
