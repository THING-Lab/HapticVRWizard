using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushCursorManager : MonoBehaviour {
	public GameObject _ribbon;
	public GameObject _tube;
	
	public void SetCursor(int brushId) {
		if (brushId == 1) {
			_ribbon.SetActive(false);
			_tube.SetActive(true);
		} else if (brushId == 0) {
			_ribbon.SetActive(true);
			_tube.SetActive(false);
		}
	}

	public void SetCursorMat(Material mat) {
		_tube.GetComponent<Renderer>().material = mat;
		_ribbon.GetComponent<Renderer>().material = mat;
	}
}
