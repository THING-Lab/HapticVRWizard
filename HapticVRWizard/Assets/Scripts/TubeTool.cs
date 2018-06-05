using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeTool : MonoBehaviour, ITool {
	public GameObject tube;
	public GameObject exporter;
	public GameObject previewObj;
	public TubeDraw preview;
	public List<GameObject> allTubes = new List<GameObject>();

	void Start () {
		preview = previewObj.GetComponent<TubeDraw>();
	}

	// Maybe reset tube? or pass color info
	public void StartStroke() {}

	public void UpdateStroke(Vector3 point, float scale) {
		preview.AddPoint(point, scale);
	}

	private void AddTube(List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		GameObject newTube = (GameObject)Instantiate(tube);
		newTube.GenerateFrom(verts, tris, uvs);
		allTubes.Add(newTube);
	}

	private void RemoveTube(string id) {
		GameObject tube = allTubes.Find(t => t.GetComponent<TubeDraw>().Id == id);
		allTubes.Remove(tube);
		Destroy(tube);
	}

	public ICommand EndStroke() {
		string tubeId = System.Guid.NewGuid().ToString();
		// Reset Preview and Pass it's data to the new tube
		TubeCommand tc = new TubeCommand(
			tubeId,
			id => RemoveTube(id),
			// Does this even work? 
			id => AddTube(id, preview.Vertices, preview.Tris, preview.Uvs)
		);

		// Use an ICommand that creates these
		preview.Reset();

		// ???
		// allTubes[allTubes.Count - 1].GetComponent<TubeDraw>().CloseMesh();
	}

	public void ExportDrawing(string filename) {
		exporter.GetComponent<JSONExportManager>().ExportMeshes(allTubes, filename);
	}

	// I should move some of this to the TubeDraw script probably
	public void ImportDrawing(string filename) {
		Scene oldScene = exporter.GetComponent<JSONExportManager>().ReadFromFile(filename);

		foreach(Geometry geo in oldScene.geometries) {
			GameObject newTube = (GameObject)Instantiate(tube);
			allTubes.Add(newTube);
			allTubes[allTubes.Count - 1].GetComponent<TubeDraw>().LoadMesh(geo);
		}
	}

	// Maybe Replace the command interface with just a class?
	private class TubeCommand : ICommand {
		private Func _undo;
		private Func _redo;
		private string _id;

		public string Id { get: { return _id; } }

		public TubeCommand(string id, Func u, Func r) {
			_id = id;
			_undo = u;
			_redo = r;
		}

		void Undo() { _undo(_id); }
		void Redo() { _redo(_id); }
	}
}
