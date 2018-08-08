using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerSelectionTarget : MonoBehaviour {
	public Renderer _trackerMesh;
	public Material _trackerMat;
	public Material _idleMat;
	public Material _selectionMat;
	public Material _selectionHoverMat;
	// Use this to ref layers
	public Transform selectionParent;

	private bool _isSelected = false;
	public bool IsSelected {
		get { return _isSelected; }
		set { _isSelected = value; }
	}

	// Use this for initialization
	void Start () {
		_trackerMesh.material = _trackerMat;
		GetComponent<Renderer>().material = _idleMat;
	}

	public void SetSelectionMode(bool isSelecting) {
		if (isSelecting) {
			_trackerMesh.material = _selectionMat;
			GetComponent<Renderer>().material = _selectionMat;
		} else {
			_trackerMesh.material = _trackerMat;
			GetComponent<Renderer>().material = _idleMat;
		}

		if (_isSelected) {
			_trackerMesh.material = _selectionMat;
		}
	}

	public void SetHover(bool isHover) {
		if (isHover || _isSelected) {
			// _trackerMesh.material = _selectionHoverMat;
			GetComponent<Renderer>().material = _selectionHoverMat;
		} else {
			// _trackerMesh.material = _selectionMat;
			GetComponent<Renderer>().material = _selectionMat;
		}
	}
}
