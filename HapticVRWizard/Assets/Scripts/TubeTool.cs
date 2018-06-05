using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TubeTool : MonoBehaviour, ITool {
	public delegate void com(string id);
	public GameObject tube;
	public GameObject exporter;
	public TubeDraw preview;
	public List<GameObject> allTubes = new List<GameObject>();

	// Maybe reset tube? or pass color info
	public void StartStroke() {}

	public void UpdateStroke(Vector3 point, float scale) {
		preview.AddPoint(point, scale);
	}

	private void AddTube(string id, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
		GameObject newTube = (GameObject)Instantiate(tube);
		Debug.Log(verts.Count);
		newTube.GetComponent<TubeDraw>().GenerateFrom(verts, tris, uvs);
		newTube.GetComponent<TubeDraw>().Id = id;
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
		TubeCommand tc = new TubeCommand(tubeId, this, preview.Vertices, preview.Tris, preview.Uvs);

		// Use an ICommand that creates these
		preview.Reset();
		tc.Redo();
		return tc;

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
	
	public class TubeCommand : ICommand {
		private string _id;
		private List<Vector3> _verts;
		private List<Vector2> _uvs;
		private List<int> _tris;
		private TubeTool _tool;

		public string Id { get { return _id; } }

		public TubeCommand(string id, TubeTool tool, List<Vector3> verts, List<int> tris, List<Vector2> uvs) {
			_id = id;
			_tool = tool;
			_verts = verts;
			_tris = tris;
			_uvs = uvs;
		}

		public void Undo() {
			_tool.RemoveTube(_id);
		}
		public void Redo() {
			_tool.AddTube(_id, _verts, _tris, _uvs);
		}
	}
}
