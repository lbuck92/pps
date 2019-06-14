using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Driver : MonoBehaviour {

	private SteamVR_TrackedObject trackedObj;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	public static bool triggerPressed = false;
	public static bool startPulse = false;

	private SteamVR_Controller.Device deviceLeft {
		get {
			return SteamVR_Controller.Input ((int)trackedObj.index);
		}
	}

	private void Awake(){
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
	}

	void Update () {
		if (deviceLeft.GetPressDown (triggerButton)) {
			triggerPressed = true;
		}

		if (startPulse) {
			Data_Management.currentTime = Time.realtimeSinceStartup;
			StartCoroutine(controllerVibration(1,3999));
			startPulse = false;
		}
	}

	public static void hapticPulse(){
		startPulse = true;
	}

	IEnumerator controllerVibration(float length, float strength){

		for (float i = 0; i < length; i += Time.deltaTime) {

			deviceLeft.TriggerHapticPulse((ushort)Mathf.Lerp(0,3999,strength));
			yield return null;

		}

	}
}
