using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeTool : MonoBehaviour, ITool {
	public GameObject tube;
	public List<GameObject> allTubes = new List<GameObject>();
	private int currentObject;

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
}
