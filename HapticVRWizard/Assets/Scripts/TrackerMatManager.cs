using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMatManager : MonoBehaviour {
	public List<Renderer> _trackerModels;
	public Material _activeMat;
	public Material _inactiveMat;
	// Update is called once per frame
	public void SetActiveTrackerMat(int activeId) {
		// Is the id of a possible tracker
		if (activeId >= 0 && activeId < _trackerModels.Count) {
			for (int id = 0; id < _trackerModels.Count; id++) {
				if (id == activeId) {
					_trackerModels[id].material = _activeMat;
				} else {
					_trackerModels[id].material = _inactiveMat;
				}
			}
		} else {
			Debug.Log("Tracker ID out of bounds, was this intended... Probably yes ;)");
		}
	} 
}
