using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushDraw : StrokeDraw
{
    private List<int> CreateTriSegment(int vertCount) {
		// 4 new verts are added every time
		return new List<int> {
			vertCount - 7, vertCount - 8, vertCount - 4,
			vertCount - 7, vertCount - 4, vertCount - 3,

			vertCount - 6, vertCount - 5, vertCount - 2,
			vertCount - 2, vertCount - 5, vertCount - 1
		};
	}

	public override void AddPoint(Vector3 p, Quaternion r, float s) {
		_pointsList.Add(p);
		Vector3 v0 = new Vector3(s, 0f, 0f);
		Vector3 v1 = new Vector3(-s, 0f, 0f);
		Vector3 v2 = new Vector3(s, -0.01f, 0f);
		Vector3 v3 = new Vector3(-s, -0.01f, 0f);

		// Multiply given(controller) rotation by inverse of parent rotation so you can remove the changes
		Quaternion localRotation = Quaternion.Inverse(transform.parent.rotation) * r;
        
    	v0 = localRotation * v0;
		v1 = localRotation * v1;
		v2 = localRotation * v2;
		v3 = localRotation * v3;
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
