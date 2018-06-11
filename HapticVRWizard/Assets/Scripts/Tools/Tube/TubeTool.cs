using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TubeTool : MonoBehaviour, ITool {
	public GameObject _tube;
	public JSONExportManager _exporter;
	public TubeDraw _preview;
	// Should this be static?
	public List<GameObject> _allTubes = new List<GameObject>();

	// Maybe reset tube? or pass color info
	public void StartStroke(Transform parent) {
		_preview.transform.SetParent(parent, false);
	}

	public void UpdateStroke(Vector3 point, float scale) {
		_preview.AddPoint(point, scale);
	}

	public void AddTube(string id, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent) {
		GameObject newTube = (GameObject)Instantiate(_tube);
		newTube.GetComponent<TubeDraw>().GenerateFrom(verts, tris, uvs);
		newTube.GetComponent<TubeDraw>().Id = id;
		newTube.transform.SetParent(parent, false);
		_allTubes.Add(newTube);
	}

	public void RemoveTube(string id) {
		GameObject delTube = _allTubes.Find(t => t.GetComponent<TubeDraw>().Id == id);
		_allTubes.Remove(delTube);
		Destroy(delTube);
	}

	public ICommand EndStroke(Transform parent) {
		string tubeId = System.Guid.NewGuid().ToString();
		// Reset Preview and Pass it's data to the new tube
		TubeCommand tc = new TubeCommand(tubeId, this, _preview.Vertices, _preview.Tris, _preview.Uvs, parent);
		_preview.Reset();
		tc.Execute();

		return tc;
	}

	public void ExportDrawing(string filename) {
		_exporter.ExportMeshes(_allTubes, filename);
	}

	// I should move some of this to the TubeDraw script probably
	public void ImportDrawing(string filename) {
		Scene oldScene = _exporter.ReadFromFile(filename);

		foreach(Geometry geo in oldScene.geometries) {
			GameObject newTube = (GameObject)Instantiate(_tube);
			_allTubes.Add(newTube);
			_allTubes.Last().GetComponent<TubeDraw>().LoadMesh(geo);
		}
	}
}
