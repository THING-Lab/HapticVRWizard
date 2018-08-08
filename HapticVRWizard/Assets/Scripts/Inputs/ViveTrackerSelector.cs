using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class ViveTrackerSelector : MonoBehaviour {
	public float _thickness = 0.002f;

	public Color _color;
	private GameObject _pointer;
	private bool _isSelectingTracker = false;
	public bool IsSelectingTracker {
		get { return _isSelectingTracker; }
	}

	public UnityEvent _startSelection;
	public UnityEvent _stopSelection;
	private int _trackerLayerMask = (1 << 8);

	private SteamVR_TrackedObject _trackedObj;
	private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }


	// Use this for initialization
	void Start () {
		_trackedObj = GetComponent<SteamVR_TrackedObject>();
		
		// MAYBE create some prefabs for these
		// Create laser ray visualization mesh
		// This is probs a silly way to do things, maybe just do a GL line or something
		_pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_pointer.transform.SetParent(transform, false);
		_pointer.transform.localScale = new Vector3(_thickness, _thickness, 100.0f);
		_pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);
		_pointer.SetActive(false);
		Object.DestroyImmediate(_pointer.GetComponent<BoxCollider>());

		// Just create a real mat
		Material pointerMat = new Material(Shader.Find("Unlit/Texture"));
		pointerMat.SetColor("_Color", _color);
		_pointer.GetComponent<MeshRenderer>().material = pointerMat;
	}
	
	// Update is called once per frame
	void Update () {
		// Get grip press
		if(Controller.GetPressDown(EVRButtonId.k_EButton_Grip)) {
			_isSelectingTracker = true;
			_pointer.SetActive(true);
			_startSelection.Invoke();
		}

		if (Controller.GetPressUp(EVRButtonId.k_EButton_Grip)) {
			_isSelectingTracker = false;
			_pointer.SetActive(false);
			_stopSelection.Invoke();
		}

		if (_isSelectingTracker) {
			Vector3 rayPos = transform.position + new Vector3(0f, 0f, 0f);
			Vector3 rayDirection = new Vector3(0f, -5f, 1f).normalized;

			Ray ray = new Ray(rayPos, transform.TransformDirection(Vector3.forward));
			RaycastHit hitInfo;
			// Make this not infinity
			bool hasHit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _trackerLayerMask);
			if (hasHit) {
				_pointer.transform.localScale = new Vector3(_thickness, _thickness, hitInfo.distance);
				_pointer.transform.localPosition = new Vector3(0.0f, 0.0f, hitInfo.distance * 0.5f);

				hitInfo.collider.gameObject.GetComponent<ViveTrackerSelectionTarget>().SetHover();
				// Set selected id here
			} else {
				_pointer.transform.localScale = new Vector3(_thickness, _thickness, 100.0f);
				_pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);
			}

			// Loop through potential targets and set proper mat
		}
	}
}
