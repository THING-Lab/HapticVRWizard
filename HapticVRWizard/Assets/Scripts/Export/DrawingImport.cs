using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DrawingImport : MonoBehaviour {
	public GameObject _meshPrefab;
	public List<Material> _materials;
	private List<GameObject> _allTubes = new List<GameObject>();

	void Update () {
		// wtf was the point of this
		foreach(GameObject stroke in _allTubes) {
			if (stroke.transform.parent != transform.parent) {
				stroke.transform.SetParent(transform.parent, false);
			}
		}
	}
	public Material GetLoadedMat(string mat) {
		return _materials.Find(m => m.name == mat);
	}

	public void LoadMesh(JsonScene drawing) {
		foreach(Geometry geo in drawing.geometries) {
			GameObject newTube = (GameObject)Instantiate(_meshPrefab);
			newTube.transform.SetParent(transform.parent, false);
			// Some ugly long way to load a material
			newTube.GetComponent<Renderer>().material = GetLoadedMat(geo.metadata.mat);
			_allTubes.Add(newTube);
			_allTubes.Last().GetComponent<TubeDraw>().LoadMesh(geo);
		}
	}
}
