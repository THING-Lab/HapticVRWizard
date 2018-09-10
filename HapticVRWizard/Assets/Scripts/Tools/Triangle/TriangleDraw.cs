using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleDraw : StrokeDraw {
	private bool _isSettingPoint = false;
	private List<Vector3> _redoPoints;
	private GameObject _segmentPreview;
	public bool IsSettingPoint {
		get { return _isSettingPoint; }
		set { _isSettingPoint = value; }
	}

	private LineRenderer PreviewLine {
		get { return _segmentPreview.GetComponent<LineRenderer>(); }
	}

	void Start() {
		// Only used for preview
		_segmentPreview = new GameObject();
		_segmentPreview.AddComponent<LineRenderer>();
		PreviewLine.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		PreviewLine.startWidth = 0.003f;
		PreviewLine.endWidth = 0.003f;
		_segmentPreview.SetActive(false);
	}

	public void SetMaterial(Material mat) {
		gameObject.GetComponent<Renderer>().material = mat;
		// PreviewLine.material = mat;
		PreviewLine.startColor = mat.color;
		PreviewLine.endColor = mat.color;
	}

	public override void Reset() {
		base.Reset();
		_isSettingPoint = false;
	}

	public void UpdateCurrentPoint(Vector3 p) {
		if (!_isSettingPoint) {
			_isSettingPoint = true;
			AddPoint(p, Quaternion.identity, 1f);
		} else {
			_pointsList[_pointsList.Count - 1] = p;
			_verts[_verts.Count - 2] = p;
			// Make the added vectors consts?
			_verts[_verts.Count - 1] = p + new Vector3(0f, -0.001f, 0f);

			RecalucalteTris();
		}

		UpdateMesh();
	}

	private void SetTriVerts(int frontB, int frontC, int backB, int backC) {
		// first vert should be set properly
		_tris[_tris.Count - 5] = frontB;
		_tris[_tris.Count - 4] = frontC;

		_tris[_tris.Count - 2] = backB;
		_tris[_tris.Count - 1] = backC;
	}

	private void RecalucalteTris() {
		// Set the preview or something
		if (_pointsList.Count == 1) {
			_segmentPreview.SetActive(false);
			PreviewLine.SetPosition(0, _pointsList[0]);
		} else if (_pointsList.Count == 2) {
			_segmentPreview.SetActive(true);
			PreviewLine.SetPosition(1, _pointsList[1]);
		}

		if (_pointsList.Count > 3) {
			_segmentPreview.SetActive(false);
			// This is the magic
			int prevFirstVert = _tris.Count - 12;
			int currentVert = _verts.Count - 2;
			int[] prevTri = { _tris[prevFirstVert], _tris[prevFirstVert + 1], _tris[prevFirstVert + 2] };
			// Debug.Log(_tris[prevFirstVert] + " " + _tris[prevFirstVert + 2] + " " + _tris[prevFirstVert + 4]);
			int[] prevBackTri = { _tris[prevFirstVert + 3], _tris[prevFirstVert + 4], _tris[prevFirstVert + 5] };

			bool prevIsABC = (prevTri[0] > prevTri[1] && prevTri[1] > prevTri[2]);

			Vector3 triANormal = Vector3.zero;
			Vector3 triBNormal = Vector3.zero;
			if (prevIsABC) {
				Vector3 triASideA = _verts[prevTri[1]] - _verts[prevTri[0]];
				Vector3 triASideB = _verts[prevTri[2]] - _verts[prevTri[0]];
				triANormal = Vector3.Cross(triASideA, triASideB).normalized;

				Vector3 triBSideA = _verts[prevTri[0]] - _verts[currentVert];
				Vector3 triBSideB = _verts[prevTri[2]] - _verts[currentVert];
				triBNormal = Vector3.Cross(triBSideA, triBSideB).normalized;
			} else {
				Vector3 triASideA = _verts[prevTri[2]] - _verts[prevTri[0]];
				Vector3 triASideB = _verts[prevTri[1]] - _verts[prevTri[0]];
				triANormal = Vector3.Cross(triASideA, triASideB).normalized;

				Vector3 triBSideA = _verts[prevTri[0]] - _verts[currentVert];
				Vector3 triBSideB = _verts[prevTri[1]] - _verts[currentVert];
				triBNormal = Vector3.Cross(triBSideA, triBSideB).normalized;
			}

			// Flip!
			bool isFlip = (Vector3.Dot(triANormal, triBNormal) > 0);
			
			// Do I need to know if the previous one is flipped
			if (prevIsABC) {
				if (isFlip) {
					SetTriVerts(prevTri[0], prevTri[2], prevBackTri[1], prevBackTri[0]);
				} else {
					SetTriVerts(prevTri[1], prevTri[0], prevBackTri[0], prevBackTri[2]);
				}
			} else {
				if (isFlip) {
					SetTriVerts(prevTri[1], prevTri[0], prevBackTri[0], prevBackTri[2]);
				} else {
					SetTriVerts(prevTri[0], prevTri[2], prevBackTri[1], prevBackTri[0]);
				}
			}
			// Debug.Log(_verts.Count);
			// Debug.Log(_tris[_tris.Count - 6] + " " + _tris[_tris.Count - 4] + " " + _tris[_tris.Count - 2]);
		}
	}

	// Need to figure out a way to understand inheritance so I don't have to add extra params
	public override void AddPoint(Vector3 p, Quaternion r, float scale) {
		_pointsList.Add(p);
		Vector3 v0 = Vector3.zero;
		Vector3 v1 = new Vector3(0f, -0.001f, 0f);

		_verts.Add(v0 + p);
		_verts.Add(v1 + p);

		// UVs here, idk how to calculate them
		_uvs.Add(new Vector2(0, 0));
		_uvs.Add(new Vector2(0, 0));

		// Important that the first vert added is always the newest one
		if (_pointsList.Count >= 3) {
			// A B C order
			_tris.Add(_verts.Count - 2);
			_tris.Add(_verts.Count - 4);
			_tris.Add(_verts.Count - 6);

			// A C B order
			_tris.Add(_verts.Count - 1);
			_tris.Add(_verts.Count - 5);
			_tris.Add(_verts.Count - 3);
		}
	}
}
