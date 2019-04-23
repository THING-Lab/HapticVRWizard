using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveMenuReader : MonoBehaviour {
	private bool _isTouchHeld = false;
	private float _menuRotation = 0f;
	private Vector2 _prevTouch = new Vector2(0, 0);
	private SteamVR_TrackedObject _trackedObj;


    //Start Chandan Changes 

    //To read touchpad position values
    public SteamVR_Action_Vector2 TouchpadAction;

    //End Chandan Changes

    public ToolManager _toolManager;
	public Transform _menuTransform;

    // Use this for initialization
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
            Vector2 touchpadValue = TouchpadAction.GetAxis(SteamVR_Input_Sources.LeftHand);
            bool currentTouch = touchpadValue != Vector2.zero ? true : false;

            //When TouchPad button is pressed
            if(SteamVR_Actions._default.Teleport.GetStateDown(SteamVR_Input_Sources.Any))
            {
                Vector2 axis = touchpadValue;

                if (axis.x > 0.05f)
                {
                    _toolManager.Redo();
                }
                else if (axis.x < -0.05f)
                {
                    _toolManager.Undo();
                }
            }

            // Menu Rotation
            
            //bool currentTouch = Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad);
            if (currentTouch && _isTouchHeld)
            {
                Vector2 axis = touchpadValue;
                float dx = axis.x - _prevTouch.x;
                // Scale dx so rotation is more comfortable
                _menuRotation -= dx * 30;

                // Use threshold
                if (dx > 0.0001 || dx < -0.0001)
                {
                    _menuTransform.localRotation = Quaternion.AngleAxis(_menuRotation, Vector3.forward);
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
}
