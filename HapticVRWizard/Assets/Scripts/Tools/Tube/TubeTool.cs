﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TubeTool : MonoBehaviour, ITool {
	public GameObject _tube;
	public ToolManager _tools;
	public TubeDraw _preview;
	// Should this be static?
	public List<GameObject> _allTubes = new List<GameObject>();

	// Maybe reset tube? or pass color info
	public void StartStroke(Transform parent, Material mat) {
		_preview.transform.SetParent(parent, false);
		_preview.GetComponent<Renderer>().material = mat;
	}

	public void UpdateStroke(Vector3 point, Quaternion rotation, float scale) {
		_preview.AddPoint(point, rotation, scale);
	}

	public void AddTube(string id, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent, Material mat) {
		GameObject newTube = (GameObject)Instantiate(_tube);
		newTube.GetComponent<TubeDraw>().GenerateFrom(verts, tris, uvs);
		newTube.GetComponent<TubeDraw>().Id = id;
		newTube.transform.SetParent(parent, false);
		newTube.GetComponent<Renderer>().material = mat;
		_allTubes.Add(newTube);
	}

	public void RemoveTube(string id) {
		GameObject delTube = _allTubes.Find(t => t.GetComponent<TubeDraw>().Id == id);
		_allTubes.Remove(delTube);
		Destroy(delTube);
	}

	public ICommand EndStroke(Transform parent, Material mat) {
		string tubeId = System.Guid.NewGuid().ToString();
		// Reset Preview and Pass it's data to the new tube
		TubeCommand tc = new TubeCommand(tubeId, this, _preview.Vertices, _preview.Tris, _preview.Uvs, parent, mat);
		_preview.Reset();
		tc.Execute();

		return tc;
	}

	// I should move some of this to the TubeDraw script probably
	public void ImportDrawing(JsonScene drawing, Transform parent) {
		foreach(Geometry geo in drawing.geometries) {
			GameObject newTube = (GameObject)Instantiate(_tube);
			newTube.transform.SetParent(parent, false);
			// Some ugly long way to load a material
			newTube.GetComponent<Renderer>().material = _tools.GetLoadedMat(geo.metadata.mat);
			_allTubes.Add(newTube);
			_allTubes.Last().GetComponent<TubeDraw>().LoadMesh(geo);
		}
	}
}
