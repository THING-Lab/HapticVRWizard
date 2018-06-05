using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCommand : ICommand {
	public void Undo() {}
	public void Redo() {}
}

public class LineTool : MonoBehaviour, ITool {
	public GameObject line;
	public List<GameObject> allLines = new List<GameObject>();
	private int currentObject;

	public void StartStroke() {
		GameObject newLine = (GameObject)Instantiate(line);
		allLines.Add(newLine);
	}

	public void UpdateStroke(Vector3 point, float scale) {
		allLines[allLines.Count - 1].GetComponent<LineDraw>().addPoint(point);
	}

	// This needs to change
	public ICommand EndStroke() {
		return new LineCommand();
	}
}
