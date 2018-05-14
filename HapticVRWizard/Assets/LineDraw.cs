using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour {
	private LineRenderer line;
	public bool isMousePressed;
	private List<Vector3> pointsList;
	private Vector3 mousePos;

	// Use this for initialization
	void Start () {

		line = gameObject.AddComponent<LineRenderer>();
		line.material = new Material(Shader.Find("Particles/Additive"));
		line.SetVertexCount(0);
		line.SetWidth(0.1f, 0.1f);
		line.SetColors(Color.green, Color.green);
		line.useWorldSpace = true;    
		isMousePressed = false;
		pointsList = new List<Vector3>();
	}

	// Update is called once per frame
	void Update () {
		// Determine mouse input state, works only on the frame it's pressed :(
		// this clears out the line buffer :(
		if (Input.GetMouseButtonDown(0)) {
			isMousePressed = true;
			line.SetVertexCount(0);
			pointsList.RemoveRange(0, pointsList.Count);
		}

		if (Input.GetMouseButtonUp(0)) {
			isMousePressed = false;
		}

		// Drawing line when mouse is moving
		if (isMousePressed) {
			// Get mouse position
			float my = Camera.main.pixelHeight - Input.mousePosition.y;
			float mx = Camera.main.pixelWidth - Input.mousePosition.x;
			mousePos = Camera.main.ScreenToWorldPoint(new Vector3(mx, my, -1f));
			mousePos.z = 0;

			// This is just checking to see if the mouse moved, should change to threshold
			if (!pointsList.Contains(mousePos)) {
				pointsList.Add(mousePos);
				line.SetVertexCount(pointsList.Count);
				line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
			}
		}
	}
}
