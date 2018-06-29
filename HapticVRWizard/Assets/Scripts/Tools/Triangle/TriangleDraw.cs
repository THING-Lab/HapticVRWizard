using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleDraw : StrokeDraw {
	private bool _isSettingPoint = false;
	public bool IsSettingPoint {
		get { return _isSettingPoint; }
		set { _isSettingPoint = value; }
	}

	public void UpdateCurrentPoint(Vector3 p) {
		if (!_isSettingPoint) {
			AddPoint(p, Quaternion.identity, 1f);
			_isSettingPoint = true;
		} else {
			_verts[_verts.Count - 1] = p;
			// Make the added vectors consts?
			_verts[_verts.Count - 2] = p + new Vector3(0f, -0.001f, 0f);
		}

		UpdateMesh();
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
