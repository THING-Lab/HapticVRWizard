using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ViveTrackerSelector : MonoBehaviour {
	public GameObject _targetRoot;
	public float _thickness = 0.002f;
	public Color _color;
	private GameObject _pointer;
	private bool _isSelectingTracker = false;
	public bool IsSelectingTracker {
		get { return _isSelectingTracker; }
	}

	private int _trackerLayerMask = (1 << 8);

	private SteamVR_TrackedObject _trackedObj;

	private List<GameObject> Targets {
		get {
			List<GameObject> selectionTargets = new List<GameObject>();
			foreach (Transform target in _targetRoot.GetComponentsInChildren<Transform>()) {
				if (target.tag == "TargetLayer" || target.tag == "TargetVisibility") {
					selectionTargets.Add(target.gameObject);
				}
			}

			return selectionTargets;
		}
	}

    // Use this for initialization
    void Start()
    {
        try
        {
            _trackedObj = GetComponent<SteamVR_TrackedObject>();

            // MAYBE create some prefabs for these
            // Create laser ray visualization mesh
            // This is probs a silly way to do things, maybe just do a GL line or something
            _pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _pointer.transform.SetParent(transform, false);
            _pointer.transform.localScale = new Vector3(_thickness, _thickness, 100.0f);
            _pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);
            _pointer.SetActive(false);
            UnityEngine.Object.DestroyImmediate(_pointer.GetComponent<BoxCollider>());

            // Just create a real mat
            // The color here doesn't work at all
            Material pointerMat = new Material(Shader.Find("Unlit/Texture"));
            pointerMat.SetColor("_Color", _color);
            _pointer.GetComponent<MeshRenderer>().material = pointerMat;
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
            // Get grip press
            bool gripPressedDown = SteamVR_Actions._default.GrabGrip.GetStateDown(SteamVR_Input_Sources.Any);
            if (gripPressedDown)
            {
                _isSelectingTracker = true;
                _pointer.SetActive(true);
                GetComponent<ViveStrokeReader>().IsSelectorMode = true;
                foreach (GameObject target in Targets)
                {
                    // Using an interface here could cut a couple lines
                    if (target.tag == "TargetLayer")
                    {
                        target.GetComponent<ViveTrackerSelectionTarget>().SetSelectionMode(_isSelectingTracker);
                    }
                    else if (target.tag == "TargetVisibility")
                    {
                        target.GetComponent<ViveTrackerLayerVisibilityTarget>().SetSelectionMode(_isSelectingTracker);
                    }
                }
            }

            //if (Controller.GetPressUp(EVRButtonId.k_EButton_Grip))
            bool gripPressedUp = SteamVR_Actions._default.GrabGrip.GetStateUp(SteamVR_Input_Sources.Any);
            if (gripPressedUp)
            {
                _isSelectingTracker = false;
                _pointer.SetActive(false);
                // Literally copied and pasted from the thing above -.-
                GetComponent<ViveStrokeReader>().IsSelectorMode = false;
                foreach (GameObject target in Targets)
                {
                    if (target.tag == "TargetLayer")
                    {
                        target.GetComponent<ViveTrackerSelectionTarget>().SetSelectionMode(_isSelectingTracker);
                    }
                    else if (target.tag == "TargetVisibility")
                    {
                        target.GetComponent<ViveTrackerLayerVisibilityTarget>().SetSelectionMode(_isSelectingTracker);
                    }
                }
            }

            int targetId = 0;
            bool selectPressed = false;

            if (_isSelectingTracker)
            {
                Vector3 rayPos = transform.position + new Vector3(0f, 0f, 0f);
                Vector3 rayDirection = new Vector3(0f, -5f, 1f).normalized;

                Ray ray = new Ray(rayPos, transform.TransformDirection(Vector3.forward));
                RaycastHit hitInfo;

                // Make this not infinity
                bool hasHit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _trackerLayerMask);
                if (hasHit)
                {
                    _pointer.transform.localScale = new Vector3(_thickness, _thickness, hitInfo.distance);
                    _pointer.transform.localPosition = new Vector3(0.0f, 0.0f, hitInfo.distance * 0.5f);

                    // Set selected id here
                    targetId = hitInfo.collider.gameObject.GetInstanceID();
                }
                else
                {
                    _pointer.transform.localScale = new Vector3(_thickness, _thickness, 100.0f);
                    _pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);
                }

                // Trigger pressed
                if (SteamVR_Actions._default.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any) && hasHit)
                {
                    // Determine whether the collider is toggle visibility or layer here
                    selectPressed = true;
                }

                // Loop through potential targets and set proper mat
                foreach (GameObject target in Targets)
                {
                    if (target.GetInstanceID() == targetId)
                    {
                        if (selectPressed)
                        {
                            // Here we need to set the proper target
                            if (target.tag == "TargetLayer")
                            {
                                GetComponent<ViveStrokeReader>().DrawParent = target.GetComponent<ViveTrackerSelectionTarget>().selectionParent;
                                target.GetComponent<ViveTrackerSelectionTarget>().IsSelected = true;
                            }

                            if (target.tag == "TargetVisibility")
                                target.GetComponent<ViveTrackerLayerVisibilityTarget>().ToggleLayerActive();
                        }
                        else
                        {
                            if (target.tag == "TargetLayer")
                                target.GetComponent<ViveTrackerSelectionTarget>().SetHover(true);
                        }
                    }
                    else
                    {
                        if (selectPressed)
                        {
                            if (target.tag == "TargetLayer")
                                target.GetComponent<ViveTrackerSelectionTarget>().IsSelected = false;
                        }
                        if (target.tag == "TargetLayer")
                            target.GetComponent<ViveTrackerSelectionTarget>().SetHover(false);
                    }
                }
            }
        }

        catch (Exception e)
        {
            Debug.Log("!!!!!!!Exception occured!!!!!!!");
            Debug.LogException(e, this);
        }
    }
}
