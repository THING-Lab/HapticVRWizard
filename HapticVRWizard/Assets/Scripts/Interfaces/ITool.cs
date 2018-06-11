using UnityEngine;

public interface ITool {
	void StartStroke(Transform parent);
	void UpdateStroke(Vector3 point, float scale);
	ICommand EndStroke(Transform parent);
}
