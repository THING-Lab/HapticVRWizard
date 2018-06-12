using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class JSONExportManager : MonoBehaviour {
	public List<GameObject> _drawParents;
	public TubeTool _tubeLoader;

	public void SaveScene() {
		// get all mesh groups and generate file names
		string date = System.DateTime.Now.ToString()
			.Replace(" ", "_")
			.Replace("/", "-")
			.Replace(":", ".");
		
		string folderName = Application.dataPath + "/Drawings/drawing_" + date;
		Directory.CreateDirectory(folderName);
		
		// For loop here
		foreach (GameObject drawing in _drawParents) {
			string fileName = folderName + "/" + drawing.name + ".json";
			List<GameObject> children = new List<GameObject>();

			foreach (Transform child in drawing.transform) {
				if (child.gameObject.tag != "NotExportable") {
					children.Add(child.gameObject);
				}
			}

			if (children.Count > 0) {
				ExportMeshes(children, fileName);
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
		
		foreach (FileInfo file in drawFiles) {
			Scene scene = ReadFromFile(file.FullName);
			_tubeLoader.ImportDrawing(
				scene,
				_drawParents.Find(o => o.name == file.Name.Replace(".json", "")).transform
			);
		}
	}
	public void ExportMeshes(List<GameObject> objects, string filename) {
		// string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", meshName, "obj");
		// string fileName = Application.dataPath + "/" + gameObject.name + ".obj"; // you can also use: "/storage/sdcard1/" +gameObject.

		Scene currentScene = new Scene();
		foreach(GameObject geo in objects) {
			Transform t = geo.transform; 
			Vector3 originalPosition = t.position;
			t.position = Vector3.zero;

			currentScene.AddGeometry(t);

			t.position = originalPosition;
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

	public Scene ReadFromFile(string filename) {
		string sceneText = File.ReadAllText(filename).Replace("object", "sceneObject");
		return JsonUtility.FromJson<Scene>(sceneText);
	}
}
