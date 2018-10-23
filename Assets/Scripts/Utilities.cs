using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Utilities : MonoBehaviour {
    private string[] interests;
    // Use this for initialization
    void Start ()
    {
        interests = ReadInterestsToList();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    public string[] GetInterests()
    {
        return interests;
    }

    private string[] ReadInterestsToList()
    {
        string path = "Assets/Resources/Interests.txt";
        //Read the text from directly from the txt file
        StreamReader reader = new StreamReader(path);
        string[] lines = reader.ReadToEnd().Split('\n');
        reader.Close();
        Debug.Log("Reading in interests to list");
        for (int i = 0; i < lines.Length; i++)
        {
            Debug.Log(lines[i].Trim());
        }
        return lines;
    }
}
