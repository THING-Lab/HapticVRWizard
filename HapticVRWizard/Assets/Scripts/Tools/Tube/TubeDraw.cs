using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TubeDraw : StrokeDraw {
	private int _numSegments = 10;

	// Should match min move distance
	public float _meshTailLength = 0.03f;

	private Vector3 _prevNormal;

	// Connects the most recently added vertex ring to the one before it
	public static List<int> CreateTriRing(int segments, int vertCount) {
		List<int> ring = new List<int>();

		for (int i = 0; i < segments; i++) {
			int i0 = vertCount - 1 - i;
			int i1 = vertCount - 1 - ((i + 1) % segments);

			int[] seg = {
				i0, i1 - segments, i0 - segments,
				i0, i1, i1 - segments
			};
			ring.AddRange(seg);
		}
		return ring;
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
	}

	public override void AddPoint(Vector3 point, Quaternion _, float scale) {
		_pointsList.Add(point);
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
			// End pinch draw copy paste

			// Clear out mesh end cap
			if (_pointsList.Count > 3) {
				_verts.RemoveRange(_verts.Count - (_numSegments), _numSegments);
				_uvs.RemoveRange(_uvs.Count - (_numSegments), _numSegments);
				_tris.RemoveRange(_tris.Count - (6 * _numSegments), 6 * _numSegments);
			}

			// Create vertex ring and Add tris
			AddVertexRing(pointIndex * _numSegments, point, direction, ringNormal, scale);
			_tris.AddRange(CreateTriRing(_numSegments, _verts.Count));

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

	// Create closing cap for mesh
	// Might want to do this in a more nuanced way instead of just creating another ring
	public void CloseMesh() {
		if (_pointsList.Count > 1) {
			Vector3 ringNormal = Vector3.zero;
			Vector3 direction = _pointsList.Last() - _pointsList[_pointsList.Count - 2];
			Vector3 scaledEndDelta = Vector3.Scale(direction.normalized, new Vector3(_meshTailLength, _meshTailLength, _meshTailLength));
			Vector3 endPoint = _pointsList.Last() + scaledEndDelta;

			AddVertexRing(_pointsList.Count * _numSegments, endPoint, endPoint, ringNormal, 0);
			_tris.AddRange(CreateTriRing(_numSegments, _verts.Count));
			UpdateMesh();
		}
	}
}
