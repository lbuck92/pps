using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Driver : MonoBehaviour
{

    public GameObject female_regular, male_regular, male_intimidating, female_intimidating, monster, cameraEye;
    GameObject active_object;
    Rigidbody current_body;
    float[] targetPositions = new float[5] { -1f, -0.7f, -1.5f, -1.2f, -2f };
    int[] trials = new int[5] { 1, 2, 3, 4, 5 };
    int curTargetArr, soloPosition, currentAvatar, increment_counter = 0;
    public static int trialNumber = 1;
    public static string active_character;
    public static float active_distance;
    public static bool trigger_pressed, avatar_walking, can_start, can_increment, can_mod, can_mod2 = false;
    public static bool trial_incomplete = true;

    void Update()
    {

        Debug.Log(trialNumber);

        if (Input.GetKeyDown(KeyCode.S)) {
            can_start = true;
            shuffleTrials(trials);
        }
        
        if(increment_counter == 4){
            can_mod = true;
            increment_counter = 0;
        }

        //quit the application when you've reached the final trial
        if (trialNumber == 126){
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
      			Application.Quit();
#endif
        }else if(trialNumber % 5 == 0 && can_mod){
            can_mod = false;
            shuffleTrials(trials);
            curTargetArr++;
            soloPosition = 0;
            if (curTargetArr == 5)
                curTargetArr = 0;
        }

        
        currentAvatar = trials[soloPosition];

        if (currentAvatar == 1){
            active_object = female_regular;
        }else if (currentAvatar == 2){
            active_object = male_regular;
        }else if (currentAvatar == 3){
            active_object = male_intimidating;
        }else if (currentAvatar == 4){
            active_object = female_intimidating;
        }else if (currentAvatar == 5){
            active_object = monster;
        }

        if (active_object != null)
            active_character = active_object.name;

        if (!trigger_pressed && !avatar_walking && can_start) 
            StartCoroutine(waitforwalk());
       


        //character reset to original position with velocity 0 when trigger is pressed and increments the trial number
        if (Controller_Driver.triggerPressed && !trigger_pressed)
        {
            trigger_pressed = true;
            can_increment = true;
            increment_counter++;
            trialNumber++;
            soloPosition++;
            characterReset();
        }

        //disallows the user from pressing the trigger prematurely before the next trial
        if (trigger_pressed)
            StartCoroutine(waitfornextresponse());

        if (!trigger_pressed && active_object.transform.position.x > 3)
            characterReset();

        updateCharacterPos();
    }

    void driveCharacterForward()
    {
        current_body = active_object.GetComponent<Rigidbody>();
        current_body.velocity = ((transform.position * -0.75f) / transform.position.magnitude);
        avatar_walking = true;
    }

    void characterReset()
    {
        active_object.transform.position = new Vector3(-3.5f, active_object.transform.position.y, active_object.transform.position.z);
        current_body.velocity = new Vector3(0f, 0f, 0f);
        if (Controller_Driver.triggerPressed)
        {
            Data_Management.recordData();
            Controller_Driver.triggerPressed = false;
        }
        avatar_walking = false;
    }

    void updateCharacterPos()
    {

        if (active_object.transform.position.x > (targetPositions[curTargetArr] - 0.025) &&
            active_object.transform.position.x < (targetPositions[curTargetArr] + 0.025))
        {
            Controller_Driver.hapticPulse();
            active_distance = targetPositions[curTargetArr];
        }
    }

    public static string getAndReturnActiveCharacter()
    {

        return active_character;

    }

    void shuffleTrials(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            int newPosition = Random.Range(0, 4);
            int tmp = arr[i];
            arr[i] = arr[newPosition];
            arr[newPosition] = tmp;
        }
    }

    IEnumerator waitfornextresponse()
    {

        yield return new WaitForSeconds(1.5f);
        trigger_pressed = false;
    }

    IEnumerator waitforwalk()
    {

        yield return new WaitForSeconds(1.5f);
        driveCharacterForward();
    }

}