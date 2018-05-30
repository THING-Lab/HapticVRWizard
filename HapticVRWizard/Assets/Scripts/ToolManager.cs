using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

		string filename = Application.dataPath + "/TempSaveFile.json";
		if(Input.GetKeyDown(KeyCode.E)) {
			tubeManager.GetComponent<TubeTool>().ExportDrawing(filename);
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			tubeManager.GetComponent<TubeTool>().ImportDrawing(filename);
		}
	}
}
