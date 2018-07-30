using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour {
	public List<DrawingImport> loadTargets;
	public string assetFolder;
	private int timer = 0;

	private bool _notLoaded = true;
	void Start () {
		
	}

	void Update () {
		timer += 1;
		if (_notLoaded && timer > 500) {
			_notLoaded = false;
			string folderPath = Path.Combine(Application.streamingAssetsPath, assetFolder);

			if (Directory.Exists(folderPath)) {
				foreach (string fileName in Directory.GetFiles(folderPath, "*.json")) {
					string sceneText = File.ReadAllText(fileName).Replace("object", "sceneObject");
					JsonScene loadedModel = JsonUtility.FromJson<JsonScene>(sceneText);

					foreach (DrawingImport target in loadTargets) {
						string targetId = target.transform.parent.gameObject.GetComponent<TrackerIdGet>().DeviceID;
						Debug.Log(target.transform.parent.gameObject.name);
						Debug.Log(targetId + " --- " + loadedModel.metadata.deviceId);

						if (targetId == loadedModel.metadata.deviceId) {
							target.LoadMesh(loadedModel);
						}
					}
				}
			}
		}
	}
}
