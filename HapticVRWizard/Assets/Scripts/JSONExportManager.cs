using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class JSONExportManager : MonoBehaviour {
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
