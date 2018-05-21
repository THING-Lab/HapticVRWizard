using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour {
	private LineRenderer line;
	private List<Vector3> pointsList = new List<Vector3>();

	// Use this for initialization
	void Awake() {
		line = this.GetComponent<LineRenderer>();
		line.material = new Material(Shader.Find("Particles/Additive"));
	}

	public void addPoint(Vector3 point) {
		pointsList.Add(point);
		line.positionCount = pointsList.Count;
		line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
	}
}
