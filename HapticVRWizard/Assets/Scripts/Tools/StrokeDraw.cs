using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StrokeDraw : MonoBehaviour {
	private Mesh _mesh;
	private MeshFilter _meshFilter;
	protected List<Vector3> _pointsList = new List<Vector3>();
	protected string _id;

	// Mesh Props?
	protected List<Vector3> _verts = new List<Vector3>();
	protected List<int> _tris = new List<int>();
	protected List<Vector2> _uvs = new List<Vector2>();

	public string Id {
		get { return _id; }
		set { _id = value; }
	}

	// Clone the lists for Commands
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

	public virtual void Reset() {
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

	protected void UpdateMesh() {
		_mesh.vertices = _verts.ToArray();
		_mesh.triangles = _tris.ToArray();
		_mesh.SetUVs(0, _uvs);
		_mesh.RecalculateBounds();
		_mesh.RecalculateNormals();
	}

	abstract public void AddPoint(Vector3 point, Quaternion r, float scale);
}
