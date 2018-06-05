using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineDraw : MonoBehaviour {
	private LineRenderer _line;
	private List<Vector3> _pointsList = new List<Vector3>();

	// Use this for initialization
	void Awake() {
		_line = gameObject.GetComponent<LineRenderer>();
		_line.material = new Material(Shader.Find("Particles/Additive"));
	}

	public void AddPoint(Vector3 point) {
		_pointsList.Add(point);
		_line.positionCount = _pointsList.Count;
		_line.SetPosition(_pointsList.Count - 1, (Vector3)_pointsList.Last());
	}
}
