﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Might need to namespace this :/
[System.Serializable]
public class JsonScene {
	public SceneMetadata metadata = new SceneMetadata();
	public List<Geometry> geometries = new List<Geometry>();
	public SceneObject sceneObject = new SceneObject();

	public JsonScene(string dId) {
		metadata.deviceId = dId;
	}

	public void AddGeometry(Transform t) {
		string uuid = System.Guid.NewGuid().ToString();
		geometries.Add(new Geometry(t, uuid));

		// Need to add a new scene object
		sceneObject.AddChild(uuid);
	}
}

[System.Serializable]
public class SceneMetadata {
  public double version = 4.5;
  public string type = "Object";
  public string generator = "Object3D.toJSON";
  public string deviceId = null;
  // Layer vis objects go here
  // [{ layerId: 0, visible: true }]
  // On loading in demo only load visible layers
}

[System.Serializable]
public class SceneObject {
	public string uuid = "378FAA8D-0888-4249-8701-92D1C1F37C51";
	public string type = "Scene";
	public int[] matrix = {1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1};
	public List<ObjectData> children = new List<ObjectData>();

	public void AddChild(string geoId) {
		children.Add(new ObjectData(geoId));
	}
}

[System.Serializable]
public class ObjectData {
	public string uuid = "E7B44C44-DD75-4C29-B571-21AD6AEF0CA9";
	public string name = "Temp Name";
	public string type = "Mesh";
	public string geometry = "";

	public ObjectData(string geoId) {
		geometry = geoId;
	}
}

[System.Serializable]
public class Geometry {
	public GeometryMetadata metadata;
	public string uuid =  "";
  	public string type = "BufferGeometry";
	public GeometryData data = new GeometryData();

	public Geometry(Transform t, string id) {
		data.attributes.SetAttributes(t);
		uuid = id;
	
		string mat = t.GetComponent<Renderer>().material.name.Split(' ')[0];
		metadata = new GeometryMetadata(mat);
	}
}

[System.Serializable]
public class GeometryMetadata {
	public double version = 4.5;
	public string type = "BufferGeometry";
	public string generator = "BufferGeometry.toJSON";
	public string mat = "ShadedWhite";
	// Layer Id goes here

	public GeometryMetadata(string meshMat) {
		mat = meshMat;
	}
}

[System.Serializable]
public class GeometryData {
	public GeometryAttrs attributes = new GeometryAttrs();
}

[System.Serializable]
public class GeometryAttrs {
	public GeometryAttribute position = new GeometryAttribute();
	public GeometryAttribute normal = new GeometryAttribute();
	public GeometryAttribute uvs = new GeometryAttribute();

	public void SetAttributes(Transform t) {
		uvs.itemSize = 2;
		MeshFilter mf = t.GetComponent<MeshFilter>();
		Mesh m = mf.sharedMesh;

		Vector3 s = t.localScale;
		Vector3 p = t.localPosition;
		Quaternion r = t.localRotation;

		for (int material = 0; material < m.subMeshCount; material ++) {
			// For material saving as mat files in unity O.o
			// Material tempMat = new Material(mats[material].shader);
			// tempMat.CopyPropertiesFromMaterial(mats[material]);
			// AssetDatabase.CreateAsset(tempMat, "Assets/Gen/TempMat.mat");

			int[] triangles = m.GetTriangles(material);
			for (int i=0; i < triangles.Length; i += 1) {
				Vector3 v = m.vertices[triangles[i]];
				position.array.Add(v.x);
				position.array.Add(v.y);
				position.array.Add(v.z);

				Vector3 n = m.normals[triangles[i]];
				normal.array.Add(n.x);
				normal.array.Add(n.y);
				normal.array.Add(n.z);

				Vector2 uv = m.uv[triangles[i]];
				uvs.array.Add(uv.x);
				uvs.array.Add(uv.y);
			}
		}
	}
}

[System.Serializable]
public class GeometryAttribute {
	public int itemSize = 3;
	public string type = "Float32Array";
	public List<float> array = new List<float>();
	public bool normalized = false;
}
