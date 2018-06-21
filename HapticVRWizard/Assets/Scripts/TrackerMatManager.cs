using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMatManager : MonoBehaviour {
	public List<Renderer> _trackerModels;
	public Material _activeMat;
	public Material _inactiveMat;
	// Update is called once per frame
	public void SetActiveTrackerMat(int activeId) {

		for (int id = 0; id < _trackerModels.Count; id++) {
			_trackerModels[id].material = _inactiveMat;

			// Is the id of a possible tracker
			if (activeId >= 0 && activeId < _trackerModels.Count && id == activeId) {
				_trackerModels[id].material = _activeMat;
			}
		}
	} 
}
