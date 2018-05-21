using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReader : MonoBehaviour {
	public bool isMousePressed;
	private Vector3 lastPos;
	public GameObject toolManager;

	// Use this for initialization
	void Start () {
		isMousePressed = false;
		lastPos = new Vector3(-1000, -1000, -1000);
	}
	
	// Update is called once per frame
	void Update () {
		// Determine mouse input state, works only on the frame it's pressed :(
		// this clears out the line buffer :(
		if (Input.GetMouseButtonDown(0)) {
			isMousePressed = true;
			toolManager.GetComponent<LineManager>().StartStroke();
		}

		if (Input.GetMouseButtonUp(0)) {
			isMousePressed = false;
		}

		// Drawing line when mouse is moving
		if (isMousePressed) {
			// Get mouse position
			float my = Camera.main.pixelHeight - Input.mousePosition.y;
			float mx = Camera.main.pixelWidth - Input.mousePosition.x;
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mx, my, -1f));
			mousePos.z = 0;

			// Add something to check for mouse movement so we don't have extra points
			toolManager.GetComponent<LineManager>().UpdateStroke(mousePos);
		}
	}
}
