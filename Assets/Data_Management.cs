using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Data_Management : MonoBehaviour {

	public static string fileName, subjectNumber;
	public string getFileName, getSubjectNumber;
	public static float currentTime;

	void Awake(){

		fileName = getFileName;
		subjectNumber = getSubjectNumber;
		//create file if necessary and record subject number, date + time
		if (!File.Exists (fileName)) 
			File.Create (fileName);

		File.WriteAllText (fileName, "Subject #:" + subjectNumber + System.Environment.NewLine + 
			System.DateTime.Now + System.Environment.NewLine + System.Environment.NewLine);
	}

	public static void recordData(){

		File.AppendAllText(fileName, "Character: " + Character_Driver.getAndReturnActiveCharacter() + System.Environment.NewLine +
			"Distance: " + Character_Driver.active_distance + " Response time in seconds: " + (Time.realtimeSinceStartup-currentTime) +
			System.Environment.NewLine);

	}

}
