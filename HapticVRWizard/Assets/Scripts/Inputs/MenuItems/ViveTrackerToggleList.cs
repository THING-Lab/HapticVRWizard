using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveTrackerToggleList : MonoBehaviour {
	public List<GameObject> _trackers;
	public List<ViveMenuToggle> _toggles;
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < _trackers.Count; i++) {
			if (!_trackers[i].active && _toggles[i].IsEnabled) {
				_toggles[i].Disable();
			} else if (_trackers[i].active && !_toggles[i].IsEnabled) {
				_toggles[i].Enable();
			}
		}
	}
}
