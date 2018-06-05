using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineTool : MonoBehaviour, ITool {
	public GameObject _line;
	public List<GameObject> _allLines = new List<GameObject>();
	private int _currentObject;

	public void StartStroke() {
		GameObject newLine = (GameObject)Instantiate(_line);
		_allLines.Add(newLine);
	}

	public void UpdateStroke(Vector3 point, float scale) {
		_allLines.Last().GetComponent<LineDraw>().AddPoint(point);
	}

	// This needs to change
	public ICommand EndStroke() {
		return new LineCommand();
	}
}
