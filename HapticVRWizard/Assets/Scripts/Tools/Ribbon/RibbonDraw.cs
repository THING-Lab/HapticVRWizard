using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RibbonDraw : MonoBehaviour {
	private Mesh _mesh;
	private MeshFilter _meshFilter;
	private List<Vector3> _pointsList = new List<Vector3>();
	private int _numSegments = 10;
	private string _id;

	// Mesh Props
	private List<Vector3> _verts = new List<Vector3>();
	private List<int> _tris = new List<int>();
	private List<Vector2> _uvs = new List<Vector2>();
	private Vector3 _prevNormal;

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

	private List<int> CreateTriSegment(int vertCount) {
		// 4 new verts are added every time
		return new List<int> {
			// vertCount - 8, vertCount - 7, vertCount - 3,
			// vertCount - 8, vertCount - 3, vertCount - 4,

			// vertCount - 3, vertCount - 7, vertCount - 8,
			// vertCount - 4, vertCount - 3, vertCount - 8,

			// vertCount - 1, vertCount - 5, vertCount - 6,
			// vertCount - 2, vertCount - 1, vertCount - 6

			vertCount - 7, vertCount - 8, vertCount - 4,
			vertCount - 7, vertCount - 4, vertCount - 3,

			vertCount - 6, vertCount - 5, vertCount - 2,
			vertCount - 2, vertCount - 5, vertCount - 1

			
			// vertCount - 4, vertCount - 2, vertCount - 1,
			// vertCount - 4, vertCount - 1, vertCount - 3
		};
	}

	// Duplicate
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

	public void AddPoint(Vector3 p, Quaternion r, float s) {
		_pointsList.Add(p);
		Vector3 v0 = new Vector3(s, 0f, 0f);
		Vector3 v1 = new Vector3(-s, 0f, 0f);
		Vector3 v2 = new Vector3(s, -0.001f, 0f);
		Vector3 v3 = new Vector3(-s, -0.001f, 0f);

		// Mapybe just pass the quat
		Quaternion rotation = Quaternion.Euler(r.x, r.y, r.z);
		// Matrix4x4 m = Matrix4x4.Rotate(rotation);
        
    	v0 = r * v0;
		v1 = r * v1;
		v2 = r * v2;
		v3 = r * v3;
		// v1 = new Vector3(0f, 0f, 0f);

		_verts.Add(v0 + p);
		_verts.Add(v1 + p);
		_verts.Add(v2 + p);
		_verts.Add(v3 + p);

		// UVs here, idk how to calculate them
		_uvs.Add(new Vector2(0, 0));
		_uvs.Add(new Vector2(0, 0));
		_uvs.Add(new Vector2(0, 0));
		_uvs.Add(new Vector2(0, 0));

		if (_pointsList.Count > 1) {
			_tris.AddRange(CreateTriSegment(_verts.Count));
		}

		UpdateMesh();
	}
}
