using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleTool : MonoBehaviour, ITool {
	public GameObject _triangleStrip;
	public TriangleDraw _preview;
	public List<GameObject> _allStrips = new List<GameObject>();

	private Vector3 _nextPoint;

	public void AddStrip(string id, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent, Material mat) {
		GameObject newStrip = (GameObject)Instantiate(_triangleStrip);
		newStrip.GetComponent<TriangleDraw>().GenerateFrom(verts, tris, uvs);
		newStrip.GetComponent<TriangleDraw>().Id = id;
		newStrip.transform.SetParent(parent, false);
		newStrip.GetComponent<Renderer>().material = mat;
		_allStrips.Add(newStrip);
	}

	// Maybe reset tube? or pass color info
	public void StartStroke(Transform parent, Material mat) {
		_preview.transform.SetParent(parent, false);
		_preview.SetMaterial(mat);
	}

	public void UpdateStroke(Vector3 point, Quaternion rotation, float scale) {
		_preview.UpdateCurrentPoint(point);
	}

	public void StartNewTri() {
		_preview.IsSettingPoint = false;
	}

	public void RemoveStrip(string id) {
		GameObject delStrip = _allStrips.Find(t => t.GetComponent<TriangleDraw>().Id == id);
		_allStrips.Remove(delStrip);
		Destroy(delStrip);
	}

	public ICommand EndStroke(Transform parent, Material mat) {
		string stripId = System.Guid.NewGuid().ToString();
		// Reset Preview and Pass it's data to the new tube
		TriangleCommand tc = new TriangleCommand(stripId, this, _preview.Vertices, _preview.Tris, _preview.Uvs, parent, mat);
		_preview.Reset();
		tc.Execute();

		return tc;
	}
}
