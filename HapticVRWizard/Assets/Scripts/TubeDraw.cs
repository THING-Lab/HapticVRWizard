using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeDraw : MonoBehaviour {
	private Mesh _mesh;
	private MeshFilter _meshFilter;
	private List<Vector3> _pointsList = new List<Vector3>();
	private int _numSegments = 10;
	private float _radius = 0.05f;

	// Mesh Props?
	private List<Vector3> _verts = new List<Vector3>();
	private List<int> _tris = new List<int>();
	private List<Vector2> _uvs = new List<Vector2>();
	private Vector3 _prevNormal;

	// Use this for initialization
	void Awake() {
		_meshFilter = gameObject.GetComponent<MeshFilter>();
		_mesh = new Mesh();
		_meshFilter.mesh = _mesh;
	}

	public void addPoint(Vector3 point) {
		_pointsList.Add(point);
	
		// Basically Generates the whole mesh
		// Defaults for normal and direction
		Vector3 ringNormal = Vector3.zero;
		Vector3 direction = point;
		int pointIndex = _pointsList.Count - 1;

		// If there is only one point just create initial ring
		// Else do all mesh generation steps
		if(_pointsList.Count == 1) {
			addVertexRing(pointIndex * _numSegments, point, direction, ringNormal);
		} else {
			// Correct direction if more than 1 point so far
			direction = point - _pointsList[pointIndex - 1];

			// This code is from Pinch draw and I need to comment it properly
			if(_pointsList.Count == 2) {
				float angleToUp = Vector3.Angle(direction, Vector3.up);

				if (angleToUp < 10 || angleToUp > 170) {
					ringNormal = Vector3.Cross(direction, Vector3.right);
				} else {
					ringNormal = Vector3.Cross(direction, Vector3.up);
				}

				ringNormal = ringNormal.normalized;
			} else {
				Vector3 prevPerp = Vector3.Cross(_pointsList[pointIndex - 1] - _pointsList[pointIndex - 2], _prevNormal);
				ringNormal = Vector3.Cross(prevPerp, point - _pointsList[pointIndex - 1]).normalized;
			}
			// End pinch draw copy pasta

			// Create vertex ring and Add tris
			addVertexRing(pointIndex * _numSegments, point, direction, ringNormal);
			addTriRing();

			// Update mesh
			_mesh.vertices = _verts.ToArray();
			_mesh.triangles = _tris.ToArray();
			_mesh.SetUVs(0, _uvs);
			// _mesh.SetColors(_colors);
			_mesh.RecalculateBounds();
			_mesh.RecalculateNormals();
		}

		_prevNormal = ringNormal;
	}

	private void addVertexRing(int offset, Vector3 ringPosition, Vector3 direction, Vector3 normal) {
		for (int i = 0; i < _numSegments; i++) {
			float angle = 360.0f * (i / (float)(_numSegments));
			Quaternion rotator = Quaternion.AngleAxis(angle, direction.normalized);
			Vector3 ringSpoke = rotator * normal.normalized * _radius;

			_verts.Add(ringPosition + ringSpoke);
			// What is that stuff with UVs and Colors?!?!
			_uvs.Add(new Vector2(i / (_numSegments - 1.0f), 0));
			// _colors.Add(ColorManager.Instance.GetCurrentColor());
		}
	}

	// Connects the most recently added vertex ring to the one before it
	public void addTriRing() {
		for (int i = 0; i < _numSegments; i++) {
			int i0 = _verts.Count - 1 - i;
			int i1 = _verts.Count - 1 - ((i + 1) % _numSegments);

			_tris.Add(i0);
			_tris.Add(i1 - _numSegments);
			_tris.Add(i0 - _numSegments);

			_tris.Add(i0);
			_tris.Add(i1);
			_tris.Add(i1 - _numSegments);
		}
	}
}
