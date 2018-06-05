using UnityEngine;

public interface ITool {
	void StartStroke();
	void UpdateStroke(Vector3 point, float scale);
	ICommand EndStroke();
}
