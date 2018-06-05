using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeDraw : MonoBehaviour {
	private Mesh _mesh;
	private MeshFilter _meshFilter;
	private List<Vector3> _pointsList = new List<Vector3>();
	private int _numSegments = 10;
	private float _radius = 0.01f;
	private string _id;

	// Should match min move distance
	public float _meshTailLength = 0.03f;

	// Mesh Props?
	private List<Vector3> _verts = new List<Vector3>();
	private List<int> _tris = new List<int>();
	private List<Vector2> _uvs = new List<Vector2>();
	private Vector3 _prevNormal;

	public string Id { get { return _id; } }
	// Clone the lists?
	public List<Vector3> Vertices { get { return new List<Vector3>(_verts); } }
	public List<int> Tris { get { return new List<int>(_tris); } }
	public List<Vector2> Uvs { get { return new List<Vector2>(_uvs); } }

	// Use this for initialization
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

		UpdateMesh();
	}

	public void LoadMesh(Geometry geo) {
		List<Vector3> verts = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Vector2> uvs = new List<Vector2>();

		// Get relevant data
		List<float> positions = geo.data.attributes.position.array;
		List<float> oldUvs = geo.data.attributes.uvs.array;

		int numVerts = positions.Count / 3;
		for(int i = 0; i < numVerts; i++) {
			int offset = i * 3;
			Vector3 vertex = new Vector3(positions[offset], positions[offset + 1], positions[offset + 2]);
			Vector3 uv = new Vector2(oldUvs[i * 2], oldUvs[(i * 2) + 1]);
			int vectorIndex;

			if (verts.Contains(vertex)) {
				vectorIndex = verts.IndexOf(vertex);
			} else {
				verts.Add(vertex);
				uvs.Add(uv);
				vectorIndex = verts.Count - 1; 
			}

			tris.Add(vectorIndex);
		}
		GenerateFrom(verts, tris, uvs);
		// _mesh.vertices = verts.ToArray();
		// _mesh.triangles = tris.ToArray();
		// _mesh.SetUVs(0, uvs);
		// _mesh.RecalculateBounds();
		// _mesh.RecalculateNormals();
	}

	public void GenerateFrom(List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		_verts = verts;
		_tris = tris;
		_uvs = uvs;

		UpdateMesh();
	}

	private void UpdateMesh() {
		_mesh.vertices = _verts.ToArray();
		_mesh.triangles = _tris.ToArray();
		_mesh.SetUVs(0, _uvs);
		// _mesh.SetColors(_colors);
		_mesh.RecalculateBounds();
		_mesh.RecalculateNormals();
	}

	public void AddPoint(Vector3 point, float scale) {
		_pointsList.Add(point);
	
		// Basically Generates the whole mesh
		// Defaults for normal and direction
		Vector3 ringNormal = Vector3.zero;
		Vector3 direction = point;
		int pointIndex = _pointsList.Count - 1;

		// If there is only one point just create initial ring
		// Else do all mesh generation steps
		if(_pointsList.Count == 1) {
			AddVertexRing(pointIndex * _numSegments, point, direction, ringNormal, 0);
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

			// Clear out mesh end cap
			if (_pointsList.Count > 3) {
				_verts.RemoveRange(_verts.Count - (_numSegments), _numSegments);
				_uvs.RemoveRange(_uvs.Count - (_numSegments), _numSegments);
				_tris.RemoveRange(_tris.Count - (6 * _numSegments), 6 * _numSegments);
			}

			// Create vertex ring and Add tris
			AddVertexRing(pointIndex * _numSegments, point, direction, ringNormal, scale);
			AddTriRing();

			// Add mesh end cap
			if (_pointsList.Count > 2) {
				CloseMesh();
			}

			UpdateMesh();
		}

		_prevNormal = ringNormal;
	}

	private void AddVertexRing(int offset, Vector3 ringPosition, Vector3 direction, Vector3 normal, float radiusScale) {
		for (int i = 0; i < _numSegments; i++) {
			float angle = 360.0f * (i / (float)(_numSegments));
			Quaternion rotator = Quaternion.AngleAxis(angle, direction.normalized);
			Vector3 ringSpoke = rotator * normal.normalized * radiusScale * 0.5f; // * _radius

			_verts.Add(ringPosition + ringSpoke);
			_uvs.Add(new Vector2(i / (_numSegments - 1.0f), 0));
			// What is that stuff with Colors?!?!
			// _colors.Add(ColorManager.Instance.GetCurrentColor());
		}
	}

	// Connects the most recently added vertex ring to the one before it
	// 6 * _numSegments tris
	private void AddTriRing() {
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

	// Create closing cap for mesh
	// Might want to do this in a more nuanced way instead of just creating another ring
	public void CloseMesh() {
		if (_pointsList.Count > 1) {
			Vector3 ringNormal = Vector3.zero;
			Vector3 direction = _pointsList[_pointsList.Count - 1] - _pointsList[_pointsList.Count - 2];
			Vector3 scaledEndDelta = Vector3.Scale(direction.normalized, new Vector3(_meshTailLength, _meshTailLength, _meshTailLength));
			Vector3 endPoint = _pointsList[_pointsList.Count - 1] + scaledEndDelta;

			AddVertexRing(_pointsList.Count * _numSegments, endPoint, endPoint, ringNormal, 0);
			AddTriRing();
			UpdateMesh();
		}
	}
}
