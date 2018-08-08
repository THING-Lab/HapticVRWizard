using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerSelectionTarget : MonoBehaviour {

	public Renderer _mesh;

	public Material _idleMat;
	public Material _selectionMat;
	public Material _selectionHoverMat;

	private bool _isSelected = false;
	private bool _isHovered = false;

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
			_isHovered = false;
		}
	}

	public void SetHover() {
		if (!_isHovered) {
			_isHovered = true;
			_mesh.material = _selectionHoverMat;
		}
	}
}
