using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleDraw : StrokeDraw {
	private bool _isSettingPoint = false;
	private List<Vector3> _redoPoints;
	private int[] prevVerts = { 0, 1 };
	private int[] nextPrevVerts = { 0, 1 };
	private int windingCheck = 1;
	public bool IsSettingPoint {
		get { return _isSettingPoint; }
		set { _isSettingPoint = value; }
	}

	public override void Reset() {
		base.Reset();
		_isSettingPoint = false;
		prevVerts[0] = 0;
		prevVerts[1] = 1;
		nextPrevVerts[0] = 0;
		nextPrevVerts[1] = 1;
		windingCheck = 1;
	}

	public void UpdateCurrentPoint(Vector3 p) {
		if (!_isSettingPoint) {
			_isSettingPoint = true;
			prevVerts[0] = nextPrevVerts[0];
			prevVerts[1] = nextPrevVerts[1];
			AddPoint(p, Quaternion.identity, 1f);
		} else {
			_pointsList[_pointsList.Count - 1] = p;
			_verts[_verts.Count - 2] = p;
			// Make the added vectors consts?
			_verts[_verts.Count - 1] = p + new Vector3(0f, -0.001f, 0f);

			recalucalteTris();
		}

		UpdateMesh();
	}

	private void recalucalteTris() {
		if (_pointsList.Count > 3) {
			// This is the magic
			int currentVert = _pointsList.Count - 1;

			Vector3 triASideA = _pointsList[prevVerts[0]] - _pointsList[nextPrevVerts[0]];
			Vector3 triASideB = _pointsList[prevVerts[1]] - _pointsList[nextPrevVerts[0]];
			Vector3 triANormal = Vector3.Cross(triASideA, triASideB).normalized;

			Vector3 triBSideA = _pointsList[nextPrevVerts[0]] - _pointsList[currentVert];
			Vector3 triBSideB = _pointsList[prevVerts[1]] - _pointsList[currentVert];
			Vector3 triBNormal = Vector3.Cross(triBSideA, triBSideB).normalized;

			if (Vector3.Dot(triANormal, triBNormal) > 0) {
				nextPrevVerts[1] = prevVerts[1];
				windingCheck = (windingCheck + 1) % 2;
			} else {
				nextPrevVerts[1] = prevVerts[0];
			}
			
			// Need to flip winding order
			// I can't think of a cleaner way to do this right now, I'm very sorry
			if (_pointsList.Count % 2 == 1) {
				_tris[_tris.Count - 6] = (nextPrevVerts[1] * 2);
				_tris[_tris.Count - 1] = (nextPrevVerts[1] * 2 + 1);
			} else {
				_tris[_tris.Count - 4] = (nextPrevVerts[1] * 2);
				_tris[_tris.Count - 3] = (nextPrevVerts[1] * 2 + 1);
			}
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

		if (_pointsList.Count == 3) {
			_tris.Add(_verts.Count - 6);
			_tris.Add(_verts.Count - 4);
			_tris.Add(_verts.Count - 2);

			_tris.Add(_verts.Count - 1);
			_tris.Add(_verts.Count - 3);
			_tris.Add(_verts.Count - 5);
		}

		// Only start generating faces once we have 3 points
		if (_pointsList.Count > 3) {
			// This is the magic
			int currentVert = _pointsList.Count - 1;
			nextPrevVerts[0] = _pointsList.Count - 2;
			// 1, 2, 3 = 2, 4, 6 : n * 2
			// 0, 1, 2 = 1, 3, 5 : n * 2 + 1

			nextPrevVerts[1] = _pointsList.Count - 3;
			// Need to flip winding order
			// I can't think of a cleaner way to do this right now, I'm very sorry
			if (_pointsList.Count % 2 == 1) {
				_tris.Add(nextPrevVerts[1] * 2);
				_tris.Add(nextPrevVerts[0] * 2);
				_tris.Add(currentVert * 2);

				_tris.Add(currentVert * 2 + 1);
				_tris.Add(nextPrevVerts[0] * 2 + 1);
				_tris.Add(nextPrevVerts[1] * 2 + 1);
			} else {
				_tris.Add(currentVert * 2);
				_tris.Add(nextPrevVerts[0] * 2);
				_tris.Add(nextPrevVerts[1] * 2);

				_tris.Add(nextPrevVerts[1] * 2 + 1);
				_tris.Add(nextPrevVerts[0] * 2 + 1);
				_tris.Add(currentVert * 2 + 1);
			}
		}
	}
}
