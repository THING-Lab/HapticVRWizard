using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.Text;

public class TrackerIdGet : MonoBehaviour {

	public int _trackerIndex;
	public string _deviceID;
	public string DeviceID {
		get { return _deviceID; }
	}

	// Use this for initialization
	void Awake () {
		ETrackedPropertyError error = new ETrackedPropertyError();
		List<string> trackerIds = new List<string>();

		for (int i = 0; i < 20; i++) {
			Valve.VR.ETrackedDeviceClass s = OpenVR.System.GetTrackedDeviceClass((uint)i);

			// Use actual name of class from enum, Might wanna switch to the enum
			if (s == Valve.VR.ETrackedDeviceClass.GenericTracker) {
				StringBuilder sb = new StringBuilder();

				// Open VR function for getting the tracker ID
				OpenVR.System.GetStringTrackedDeviceProperty(
					(uint)i,
					ETrackedDeviceProperty.Prop_SerialNumber_String,
					sb,
					OpenVR.k_unMaxPropertyStringSize, ref error
				);
				trackerIds.Add(sb.ToString());
			}
		}

		if (_trackerIndex + 1 <= trackerIds.Count) {
			_deviceID = trackerIds[_trackerIndex];
		}
	}
}
