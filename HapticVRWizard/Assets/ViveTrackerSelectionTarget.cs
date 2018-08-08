using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerSelectionTarget : MonoBehaviour {

	public Renderer _mesh;

	public Material _idleMat;
	public Material _selectionMat;
	public Material _selectionHoverMat;

	private bool _isSelected = false;
	public bool IsSelected {
		get { return _isSelected; }
		set { _isSelected = value; }
	}

	// Use this for initialization
	void Start () {
		_mesh.material = _idleMat;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetSelectionMode(bool isSelecting) {
		if (isSelecting || _isSelected) {
			_mesh.material = _selectionMat;
		} else {
			_mesh.material = _idleMat;
		}
	}

	public void SetHover(bool isHover) {
		if (isHover || _isSelected) {
			_mesh.material = _selectionHoverMat;
		} else {
			_mesh.material = _selectionMat;
		}
	}
}
