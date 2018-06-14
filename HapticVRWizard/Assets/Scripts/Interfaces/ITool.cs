using UnityEngine;

public interface ITool {
	void StartStroke(Transform parent);
	void UpdateStroke(Vector3 point, Vector3 rotation, float scale);
	ICommand EndStroke(Transform parent);
}
