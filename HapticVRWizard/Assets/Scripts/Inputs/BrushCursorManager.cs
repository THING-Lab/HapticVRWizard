using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushCursorManager : MonoBehaviour {
	public GameObject _ribbon;
	public GameObject _tube;
	public GameObject _brush;
	public GameObject _model;
    public GameObject _ribbon_tube_handle;

    public void SetCursor(int brushId) {
		if (brushId == 2) {
			_brush.SetActive(true);
			_ribbon.SetActive(false);
			_tube.SetActive(false);
			_model.SetActive(false);
            _ribbon_tube_handle.SetActive(false);
        }
		if (brushId == 1) {
			_brush.SetActive(false);
			_ribbon.SetActive(false);
			_tube.SetActive(true);
			_model.SetActive(true);
            _ribbon_tube_handle.SetActive(true);
        } else if (brushId == 0) {
			_brush.SetActive(false);
			_ribbon.SetActive(true);
			_tube.SetActive(false);
            _model.SetActive(true);
            _ribbon_tube_handle.SetActive(true);

        }
	}

	public void SetCursorMat(Material mat) {
		_tube.GetComponent<Renderer>().material = mat;
		_ribbon.GetComponent<Renderer>().material = mat;
		_brush.GetComponent<Renderer>().material = mat;
	}
}
