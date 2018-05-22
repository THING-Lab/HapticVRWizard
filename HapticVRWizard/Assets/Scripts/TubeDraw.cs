using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeDraw : MonoBehaviour {
	private Mesh mesh;
	private MeshFilter meshFilter;
	private List<Vector3> pointsList = new List<Vector3>();

	// Use this for initialization
	void Awake() {
		meshFilter = gameObject.GetComponent<MeshFilter>();
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		Vector3[] vertices = new Vector3[4];

		vertices[0] = new Vector3(0, 0, 0);
		vertices[1] = new Vector3(1, 0, 0);
		vertices[2] = new Vector3(0, 0, 1);
		vertices[3] = new Vector3(1, 0, 1);

		mesh.vertices = vertices;

		int[] tris = new int[6];

		tris[0] = 0;
		tris[1] = 2;
		tris[2] = 1;

		tris[3] = 2;
		tris[4] = 3;
		tris[5] = 1;

		mesh.triangles = tris;

		mesh.RecalculateNormals();
	}

	public void addPoint(Vector3 point) {
		
	}
}
