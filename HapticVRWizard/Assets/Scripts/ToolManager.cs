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
		currentTool = tubeManager.GetComponent<TubeTool>();
	}
}
