using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardShortcuts : MonoBehaviour {
	public ToolManager _tools;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)) {
			// Save
		}
		if(Input.GetKeyDown(KeyCode.D)) {
			// Load
		}

		if(Input.GetKeyDown(KeyCode.K)) {
			_tools.SetBrush(ToolManager.ToolTypes.Ribbon);
		}
		if(Input.GetKeyDown(KeyCode.L)) {
			_tools.SetBrush(ToolManager.ToolTypes.Tube);
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			_tools.SetMaterial("ShadedWhite");
		}

		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			_tools.SetMaterial("ShadedRed");
		}

		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			_tools.SetMaterial("ShadedGreen");
		}

		if(Input.GetKeyDown(KeyCode.Alpha4)) {
			_tools.SetMaterial("ShadedBlue");
		}

		if(Input.GetKeyDown(KeyCode.Alpha5)) {
			_tools.SetMaterial("ShadedOrange");
		}

		if(Input.GetKeyDown(KeyCode.Q)) {
			_tools.SetMaterial("UnlitWhite");
		}

		if(Input.GetKeyDown(KeyCode.W)) {
			_tools.SetMaterial("UnlitRed");
		}

		if(Input.GetKeyDown(KeyCode.E)) {
			_tools.SetMaterial("UnlitGreen");
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			_tools.SetMaterial("UnlitBlue");
		}

		if(Input.GetKeyDown(KeyCode.T)) {
			_tools.SetMaterial("UnlitOrange");
		}
	}
}
