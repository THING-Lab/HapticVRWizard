using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
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
		currentTool = tubeManager.GetComponent<TubeTool>();

		// Gen filename here, base it off path
		string drawingLocation = Application.dataPath + "/Drawings";
		DirectoryInfo directory = new DirectoryInfo(drawingLocation);
		IOrderedEnumerable<FileInfo> drawFiles = directory.GetFiles("*.json")
			.OrderByDescending(f => f.LastWriteTime);

		if (drawFiles.Count() > 0) {
			tubeManager.GetComponent<TubeTool>().ImportDrawing(drawFiles.First().FullName);
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.T)) {
			currentTool = tubeManager.GetComponent<TubeTool>();
		}

		if(Input.GetKeyDown(KeyCode.L)) {
			currentTool = lineManager.GetComponent<LineTool>();
		}

		if(Input.GetKeyDown(KeyCode.E)) {
			string date = System.DateTime.Now.ToString()
				.Replace(" ", "_")
				.Replace("/", "-")
				.Replace(":", ".");

			string filename = Application.dataPath + "/Drawings/drawing_" + date + ".json";
			tubeManager.GetComponent<TubeTool>().ExportDrawing(filename);
		}
	}
}
