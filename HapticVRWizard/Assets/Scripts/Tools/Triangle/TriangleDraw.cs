using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleDraw : MonoBehaviour {
	private Mesh _mesh;
	private MeshFilter _meshFilter;
	private List<Vector3> _pointsList = new List<Vector3>();
	private int _numSegments = 10;
	private string _id;
	private bool _isSettingPoint = false;
	public bool IsSettingPoint {
		get { return _isSettingPoint; }
		set { _isSettingPoint = value; }
	}
	// Mesh Props
	private List<Vector3> _verts = new List<Vector3>();
	private List<int> _tris = new List<int>();
	private List<Vector2> _uvs = new List<Vector2>();

	public string Id {
		get { return _id; }
		set { _id = value; }
	}

	// Might wanna question the existance of these props
	public List<Vector3> Vertices {
		get {
			List<Vector3> vs = new List<Vector3>();
			foreach(Vector3 v in _verts) {
				vs.Add(new Vector3(v.x, v.y, v.z));
			}
			return vs;
		}
	}

	public List<int> Tris {
		get {
			List<int> ts = new List<int>();
			foreach(int t in _tris) {
				ts.Add(t);
			}
			return ts;
		}
	}

	public List<Vector2> Uvs {
		get {
			List<Vector2> us = new List<Vector2>();
			foreach(Vector2 u in _uvs) {
				us.Add(new Vector2(u.x, u.y));
			}
			return us;
		}
	}

	void Awake() {
		_meshFilter = gameObject.GetComponent<MeshFilter>();
		_mesh = new Mesh();
		_meshFilter.mesh = _mesh;
	}

	public void Reset() {
		_pointsList = new List<Vector3>();
		_verts = new List<Vector3>();
		_tris = new List<int>();
		_uvs = new List<Vector2>();
		_mesh.Clear();
	}

	public void GenerateFrom(List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		_verts = verts;
		_tris = tris;
		_uvs = uvs;

		UpdateMesh();
	}

	// Duplicate
	private void UpdateMesh() {
		_mesh.vertices = _verts.ToArray();
		_mesh.triangles = _tris.ToArray();
		//_mesh.normals = _verts.ToArray();
		_mesh.SetUVs(0, _uvs);
		_mesh.RecalculateBounds();
		_mesh.RecalculateNormals();
	}

	public void UpdateCurrentPoint(Vector3 p) {
		if (!_isSettingPoint) {
			AddPoint(p);
			_isSettingPoint = true;
		} else {
			_verts[_verts.Count - 1] = p;
			// Make the added vectors consts?
			_verts[_verts.Count - 2] = p + new Vector3(0f, -0.001f, 0f);
		}

		UpdateMesh();
	}

	public void AddPoint(Vector3 p) {
		_pointsList.Add(p);
		Vector3 v0 = Vector3.zero;
		Vector3 v1 = new Vector3(0f, -0.001f, 0f);

		_verts.Add(v0 + p);
		_verts.Add(v1 + p);

		// UVs here, idk how to calculate them
		_uvs.Add(new Vector2(0, 0));
		_uvs.Add(new Vector2(0, 0));

		// Only start generating faces once we have 3 points
		if (_pointsList.Count > 2) {
			// Need to flip winding order
			// I can't think of a cleaner way to do this right now, I'm very sorry
			if (_pointsList.Count % 2 == 1) {
				_tris.Add(_verts.Count - 6);
				_tris.Add(_verts.Count - 4);
				_tris.Add(_verts.Count - 2);

				_tris.Add(_verts.Count - 1);
				_tris.Add(_verts.Count - 3);
				_tris.Add(_verts.Count - 5);
			} else {
				_tris.Add(_verts.Count - 2);
				_tris.Add(_verts.Count - 4);
				_tris.Add(_verts.Count - 6);

				_tris.Add(_verts.Count - 5);
				_tris.Add(_verts.Count - 3);
				_tris.Add(_verts.Count - 1);
			}
		}
	}
}
