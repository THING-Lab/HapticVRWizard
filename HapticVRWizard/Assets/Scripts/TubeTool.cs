using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temp for json test
using System.IO;

public class TubeTool : MonoBehaviour, ITool {
	public GameObject tube;
	public GameObject exporter;
	public List<GameObject> allTubes = new List<GameObject>();

	public void StartStroke() {
		GameObject newTube = (GameObject)Instantiate(tube);
		allTubes.Add(newTube);
	}

	public void UpdateStroke(Vector3 point) {
		allTubes[allTubes.Count - 1].GetComponent<TubeDraw>().AddPoint(point);
	}

	public void EndStroke() {
		allTubes[allTubes.Count - 1].GetComponent<TubeDraw>().CloseMesh();
	}

	public void ExportDrawing() {
		exporter.GetComponent<JSONExportManager>().ExportMeshes(allTubes);
	}
}
