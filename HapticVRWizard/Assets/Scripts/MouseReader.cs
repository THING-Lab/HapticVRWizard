using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReader : MonoBehaviour {
	public bool _isMousePressed;
	private Vector3 _lastPos;
	public GameObject _toolManager;
	public float _moveThreshold = 0.02f;

	// Use this for initialization
	void Start () {
		_isMousePressed = false;
		// Hack to allow for a new position on first
		_lastPos = new Vector3(-1000, -1000, -1000);
	}
	
	// Update is called once per frame
	void Update () {
		ITool currentTool = _toolManager.GetComponent<ToolManager>().CurrentTool;

		// Determine mouse input state, works only on the frame it's pressed :(
		// this clears out the line buffer :(
		if (Input.GetMouseButtonDown(0)) {
			_isMousePressed = true;
			currentTool.StartStroke();
		}

		if (Input.GetMouseButtonUp(0)) {
			_isMousePressed = false;
		}

		// Drawing line when mouse is moving
		if (_isMousePressed) {
			// Get mouse position
			float my = Camera.main.pixelHeight - Input.mousePosition.y;
			float mx = Camera.main.pixelWidth - Input.mousePosition.x;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mx, my, -1f));
			mousePos.z = 0;

			// We might need to add more sophisticated position smoothing than this
			if(Vector3.Distance(mousePos, _lastPos) >= _moveThreshold) {
				currentTool.UpdateStroke(mousePos);
				_lastPos = mousePos;
			}
		}
	}
}
