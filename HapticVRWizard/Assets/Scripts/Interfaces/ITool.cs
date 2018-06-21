using UnityEngine;

public interface ITool {
	void StartStroke(Transform parent, Material mat);
	void UpdateStroke(Vector3 point, Quaternion rotation, float scale);
	ICommand EndStroke(Transform parent, Material mat);
}
