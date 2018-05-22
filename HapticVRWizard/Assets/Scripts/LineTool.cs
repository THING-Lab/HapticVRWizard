using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTool : MonoBehaviour, ITool {
	public GameObject line;
	public List<GameObject> allLines = new List<GameObject>();
	private int currentObject;

	public void StartStroke() {
		GameObject newLine = (GameObject)Instantiate(line);
		allLines.Add(newLine);
	}

	public void UpdateStroke(Vector3 point) {
		allLines[allLines.Count - 1].GetComponent<LineDraw>().addPoint(point);
	}
}
