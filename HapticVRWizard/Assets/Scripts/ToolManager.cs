using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour {

	public GameObject lineManager;
	public GameObject tubeManager;

	private ITool currentTool;
	public ITool CurrentTool {
		get { return currentTool; }
	}

	// Use this for initialization
	void Start () {
		// Initial Tool Choice, Probs want to display this in the UI somehow
		// ENUM?
		// This could be more generic
		currentTool = tubeManager.GetComponent<TubeTool>();
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.T)) {
			currentTool = tubeManager.GetComponent<TubeTool>();
		}

		if(Input.GetKeyDown(KeyCode.L)) {
			currentTool = lineManager.GetComponent<LineTool>();
		}
	}
}
