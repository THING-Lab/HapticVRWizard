using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerLayerVisibilityTarget : MonoBehaviour {
	public GameObject _layer;
	public Material _idleMat;
	public Material _inactiveMat;
	public Material _activeMat;

	private bool _isLayerActive = true;

	// Use this for initialization
	public void ToggleLayerActive() {
		_isLayerActive = !_isLayerActive;
		_layer.SetActive(_isLayerActive);

		if (_isLayerActive) {
			GetComponent<Renderer>().material = _activeMat;
		} else {
			GetComponent<Renderer>().material = _inactiveMat;
		}
	}

	public void SetSelectionMode(bool isSelecting) {
		if (isSelecting) {
			if (_isLayerActive) {
				GetComponent<Renderer>().material = _activeMat;
			} else {
				GetComponent<Renderer>().material = _inactiveMat;
			}
		} else {
			GetComponent<Renderer>().material = _idleMat;
		}
	}
}
