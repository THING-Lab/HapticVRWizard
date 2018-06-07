using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveUIPointer : MonoBehaviour {
	public float _thickness = 0.002f;
	public float _cursorScale = 0.02f;
	public Color _color;
	private int _menuLayerMask = ~(1 << 5);

	private GameObject _hitPoint;
	private GameObject _pointer;

	private SteamVR_TrackedObject _trackedObj;
	private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)_trackedObj.index); }
    }

	// Use this for initialization
	void Start () {
		_trackedObj = GetComponent<SteamVR_TrackedObject>();

		// MAYBE create some prefabs for these
		// Create laser ray visualization mesh
		_pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_pointer.transform.SetParent(transform, false);
		_pointer.transform.localScale = new Vector3(_thickness, _thickness, 100.0f);
		_pointer.transform.localPosition = new Vector3(0.0f, 0.0f, 50.0f);

		// Create cursor for pointer menu
		_hitPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_hitPoint.transform.SetParent(transform, false);
		_hitPoint.transform.localScale = new Vector3(_cursorScale, _cursorScale, _cursorScale);
		_hitPoint.transform.localPosition = new Vector3(0.0f, 0.0f, 100.0f);

		_pointer.SetActive(false);
		_hitPoint.SetActive(false);

		Object.DestroyImmediate(_pointer.GetComponent<BoxCollider>());
		Object.DestroyImmediate(_hitPoint.GetComponent<SphereCollider>());

		Material pointerMat = new Material(Shader.Find("Unlit/Texture"));
		pointerMat.SetColor("_Color", _color);
		_pointer.GetComponent<MeshRenderer>().material = pointerMat;
		_hitPoint.GetComponent<MeshRenderer>().material = pointerMat;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
		RaycastHit hitInfo;
		// Make this not infinity
		bool hasHit = Physics.Raycast(ray, out hitInfo, Mathf.Infinity);
		// Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, _menuLayerMask)

		if (hasHit) {
			_pointer.SetActive(true);
			_hitPoint.SetActive(true);

			_pointer.transform.localScale = new Vector3(_thickness, _thickness, hitInfo.distance);
			_pointer.transform.localPosition = new Vector3(0.0f, 0.0f, hitInfo.distance * 0.5f);

			_hitPoint.transform.localPosition = new Vector3(0.0f, 0.0f, hitInfo.distance);

			// Let the stroke reader know that the menu is in control
			GetComponent<ViveStrokeReader>().IsPointerMode = true;
		} else {
			_pointer.SetActive(false);
			_hitPoint.SetActive(false);

			GetComponent<ViveStrokeReader>().IsPointerMode = false;
		}
		
	}
}
