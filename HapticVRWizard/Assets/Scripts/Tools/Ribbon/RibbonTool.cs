﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonTool : MonoBehaviour, ITool {
	public GameObject _ribbon;
	public RibbonDraw _preview;
	// Should this be static?
	public List<GameObject> _allRibbons = new List<GameObject>();

	// Maybe reset tube? or pass color info
	public void StartStroke(Transform parent, Material mat) {
		_preview.transform.SetParent(parent, false);
		_preview.GetComponent<Renderer>().material = mat;
	}

	public void UpdateStroke(Vector3 point, Quaternion rotation, float scale) {
		_preview.AddPoint(point, rotation, scale);
	}

	public void AddRibbon(string id, List<Vector3> verts, List<int> tris, List<Vector2> uvs, Transform parent, Material mat) {
		GameObject newRibbon = (GameObject)Instantiate(_ribbon);
		newRibbon.GetComponent<RibbonDraw>().GenerateFrom(verts, tris, uvs);
		newRibbon.GetComponent<RibbonDraw>().Id = id;
		newRibbon.transform.SetParent(parent, false);
		newRibbon.GetComponent<Renderer>().material = mat;
		_allRibbons.Add(newRibbon);
	}

	public void RemoveRibbon(string id) {
		GameObject delRibbon = _allRibbons.Find(t => t.GetComponent<RibbonDraw>().Id == id);
		_allRibbons.Remove(delRibbon);
		Destroy(delRibbon);
	}

	public ICommand EndStroke(Transform parent, Material mat) {
		string ribbonId = System.Guid.NewGuid().ToString();
		// Reset Preview and Pass it's data to the new tube
		RibbonCommand rc = new RibbonCommand(ribbonId, this, _preview.Vertices, _preview.Tris, _preview.Uvs, parent, mat);
		_preview.Reset();
		rc.Execute();

		return rc;
	}

	// // I should move some of this to the TubeDraw script probably
	// public void ImportDrawing(Scene drawing, Transform parent) {
	// 	foreach(Geometry geo in drawing.geometries) {
	// 		GameObject newTube = (GameObject)Instantiate(_tube);
	// 		newTube.transform.SetParent(parent, false);
	// 		_allTubes.Add(newTube);
	// 		_allTubes.Last().GetComponent<TubeDraw>().LoadMesh(geo);
	// 	}
	// }
}
