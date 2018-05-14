using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour {
	private LineRenderer line;
	public List<Vector3> pointsList;

	// Use this for initialization
	void Start() {
		line = gameObject.AddComponent<LineRenderer>();
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.positionCount = 0;
		line.startWidth = 0.01f;
		line.endWidth = 0.01f;
		line.startColor = Color.green;
		line.endColor = Color.green;
		line.useWorldSpace = true;
		pointsList = new List<Vector3>();
	}

	public void addPoint(Vector3 point) {
		pointsList.Add(point);
		line.positionCount = pointsList.Count;
		line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
	}

	// public void setPointsList(newList) {
	// 	line.SetVertexCount(0);
  //   pointsList.RemoveRange(0, pointsList.Count);

	// 	pointsList = new List<Vector3>(newList);

	// 	line.SetVertexCount(pointsList.Count);
	// 	for(int i = 0; i < pointsList.Count; i++) {
	// 		line.SetPosition(i, (Vector3)pointsList[i]);
	// 	}
	// }
}
