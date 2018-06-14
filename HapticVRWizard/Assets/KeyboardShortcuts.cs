using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour {
	public Material _mats0;
	public Material _mats1;
	public List<Color> _colors;

	public ToolManager _tools;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			_tools.SetMaterial(_mats0);
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			_tools.SetMaterial(_mats1);
		}

		if(Input.GetKeyDown(KeyCode.Q)) {
			_tools.SetColor(_colors[0]);
		}

		if(Input.GetKeyDown(KeyCode.W)) {
			_tools.SetColor(_colors[1]);
		}

		if(Input.GetKeyDown(KeyCode.E)) {
			_tools.SetColor(_colors[2]);
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			_tools.SetColor(_colors[3]);
		}
	}
}
