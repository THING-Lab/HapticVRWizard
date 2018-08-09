using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class JSONExportManager : MonoBehaviour {
	public GameObject _drawRoot;
	// Make this a generic mesh loader
	public TubeTool _tubeLoader;
	private List<GameObject> DrawParents {
		get {
			List<GameObject> parents = new List<GameObject>();
			foreach (Transform parent in _drawRoot.transform) {
				if (parent.tag == "DrawParent") {
					parents.Add(parent.gameObject);
				}
			}

			return parents;
		}
	}

	public void SaveScene() {
		// get all mesh groups and generate file names
		string date = System.DateTime.Now.ToString()
			.Replace(" ", "_")
			.Replace("/", "-")
			.Replace(":", ".");
		
		string folderName = Application.dataPath + "/Drawings/drawing_" + date;
		Directory.CreateDirectory(folderName);
		
		// For loop here
		foreach(GameObject drawParent in DrawParents) {
			string fileName = folderName + "/" + drawParent.name + ".json";
			List<GameObject> children = new List<GameObject>();

			// Find all drawyings, ignore their layers
			// Not saving layers ATM, this ain't photoshop
			foreach(StrokeDraw stroke in drawParent.GetComponentsInChildren<StrokeDraw>()) {
				if (stroke.gameObject.tag == "DrawElement") {
					children.Add(stroke.gameObject);
				}
			}

			if (children.Count > 0) {
				string deviceId = "untracked";
				if (drawParent.GetComponent<TrackerIdGet>() != null) {
					deviceId = drawParent.GetComponent<TrackerIdGet>().DeviceID;
				}

				ExportMeshes(children, fileName, deviceId);
			}
		}
	}

	public void LoadScene() {
		// Load previously saved file on startup
		// string drawingLocation = Application.dataPath + "/Drawings";
		// DirectoryInfo directory = new DirectoryInfo(drawingLocation);
		// IOrderedEnumerable<FileInfo> drawFiles = directory.GetFiles("*.json")
		// 	.OrderByDescending(f => f.LastWriteTime);

		// Maybe check if a save exists first
		string path = Application.dataPath + "/Drawings";
		FileInfo[] drawFiles = new DirectoryInfo(path)
			.GetDirectories()
			.OrderByDescending(d => d.LastWriteTimeUtc)
			.First()
			.GetFiles("*.json");
		
		// This doesn't handle for if the parent doesn't exist :/
		foreach (FileInfo file in drawFiles) {
			JsonScene scene = ReadFromFile(file.FullName);
			_tubeLoader.ImportDrawing(
				scene,
				DrawParents.Find(o => o.name == file.Name.Replace(".json", "")).transform
			);
		}
	}
	public void ExportMeshes(List<GameObject> objects, string filename, string trackerId) {
		// string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", meshName, "obj");
		// string fileName = Application.dataPath + "/" + gameObject.name + ".obj"; // you can also use: "/storage/sdcard1/" +gameObject.

		JsonScene currentScene = new JsonScene(trackerId);
		foreach(GameObject geo in objects) {
			currentScene.AddGeometry(geo.transform);
		}

		string json = JsonUtility.ToJson(currentScene);
		WriteToFile(json, filename);
		Debug.Log("Exported Mesh: " + filename);
	}
 
	public void WriteToFile(string s, string filename) {
		using (StreamWriter sw = new StreamWriter(filename)) {
			sw.Write(s.Replace("sceneObject", "object"));
		}
	}

	public JsonScene ReadFromFile(string filename) {
		string sceneText = File.ReadAllText(filename).Replace("object", "sceneObject");
		return JsonUtility.FromJson<JsonScene>(sceneText);
	}
}
