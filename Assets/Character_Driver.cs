using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Driver : MonoBehaviour {

	public GameObject female_regular, male_regular, male_intimidating, female_intimidating, monster, cameraEye;
	GameObject active_object;
	Rigidbody current_body;
	float[] targetPositions = new float[5]{-1f,-0.7f,-1.5f,-1.2f,-2f};
	int[] trials = new int[30]{1,2,3,4,5,4,2,3,1,5,3,4,5,2,1,2,3,4,5,1,5,4,3,2,1,4,5,1,2,3};
	int curTargetArr, soloPosition, currentAvatar = 0;
	public static int trialNumber = 1;
	public static string active_character;
	public static float active_distance;
	public static bool trigger_pressed, avatar_walking, can_start, can_increment = false;
	public static bool trial_incomplete = true;

	void Update () {

		if (Input.GetKeyDown (KeyCode.S))
			can_start = true;

		if (soloPosition > 29)
			cameraEye.SetActive (false);

		currentAvatar = trials [soloPosition];

		if (currentAvatar == 1) {
			active_object = female_regular;
		} else if (currentAvatar == 2) {
			active_object = male_regular;
		} else if (currentAvatar == 3) {
			active_object = male_intimidating;
		} else if (currentAvatar == 4) {
			active_object = female_intimidating;
		} else if (currentAvatar == 5) {
			active_object = monster;
		}

		if (active_object != null)
			active_character = active_object.name;

		if (!trigger_pressed && !avatar_walking && can_start) {
			StartCoroutine (waitforwalk ());
		}

		//choose current character (you can only choose current character when all trials have been run, i.e., when curTargetArr == 0)
		//remove comments if you want to run the study manually.
		/*if (Input.GetKeyDown (KeyCode.Alpha1) && curTargetArr == 0) {
			active_object = female_regular;
			active_character = active_object.name;
		} else if (Input.GetKeyDown (KeyCode.Alpha2) && curTargetArr == 0) {
			active_object = male_regular;
			active_character = active_object.name;
		} else if (Input.GetKeyDown (KeyCode.Alpha3) && curTargetArr == 0) {
			active_object = male_intimidating;
			active_character = active_object.name;
		} else if (Input.GetKeyDown (KeyCode.Alpha4) && curTargetArr == 0) {
			active_object = female_intimidating;
			active_character = active_object.name;
		} else if (Input.GetKeyDown (KeyCode.Alpha5) && curTargetArr == 0) {
			active_object = monster;
			active_character = active_object.name;
		} else if (active_object == null)
			return;*/

		//drive character forward (give character a velocity > 0)
		if (active_object != null && Input.GetKeyDown (KeyCode.Space))
			driveCharacterForward ();

		//character reset to original position with velocity 0 when trigger is pressed and increments the trial number
		if (Controller_Driver.triggerPressed && !trigger_pressed) {
			trigger_pressed = true;
			can_increment = true;
			trialNumber++;
			curTargetArr++;
			if (curTargetArr == 5) {
				curTargetArr = 0;
				soloPosition++;
			}
			characterReset ();
		}

		//disallows the user from pressing the trigger prematurely before the next trial
		if (trigger_pressed)
			StartCoroutine (waitfornextresponse ());

		if(!trigger_pressed && active_object.transform.position.x > 3)
			characterReset();

		updateCharacterPos ();
	}

	void driveCharacterForward(){
		current_body = active_object.GetComponent<Rigidbody> ();
		current_body.velocity = ((transform.position * -0.75f) / transform.position.magnitude);
		avatar_walking = true;
	}

	void characterReset(){
		active_object.transform.position = new Vector3 (-3.5f, active_object.transform.position.y, active_object.transform.position.z);
		current_body.velocity = new Vector3 (0f, 0f, 0f);
		if (Controller_Driver.triggerPressed) {
			Data_Management.recordData ();
			Controller_Driver.triggerPressed = false;
		}
		avatar_walking = false;
	}

	void updateCharacterPos(){

		if (active_object.transform.position.x > (targetPositions[curTargetArr] - 0.025) && 
			active_object.transform.position.x < (targetPositions[curTargetArr] + 0.025)) {
			Controller_Driver.hapticPulse ();
			active_distance = targetPositions [curTargetArr];
			Debug.Log ("Dist no. " + (curTargetArr+1));
			if (curTargetArr == 5) {
				Debug.Log ("MAKE A CHARACTER SWITCH");
			}
		}
	}

	public static string getAndReturnActiveCharacter(){

		return active_character;

	}

	IEnumerator waitfornextresponse(){

		yield return new WaitForSeconds (1.5f);
		trigger_pressed = false;
	}

	IEnumerator waitforwalk(){

		yield return new WaitForSeconds (1.5f);
		driveCharacterForward ();
	}
		
}
