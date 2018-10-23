using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CharacterCreation : MonoBehaviour {

    private string[] allInterests;
    private List<string> assignedInterests;
    private List<TextMesh> interestTexts;
    public int numberOfMatchInterests = 2;
    public GameObject interestsContainer;
    private static List<Vector4> interestColours;

    void Start()
    {
        interestTexts = new List<TextMesh>();
        interestTexts.Capacity = numberOfMatchInterests;
        allInterests = ReadInterestsToList();
        interestColours = GenerateRandomTextColours();
        for (int i = 0; i < numberOfMatchInterests; i++)
        {
            interestTexts.Add(interestsContainer.transform.GetComponentsInChildren<TextMesh>()[i]);
        }
        
        assignedInterests = new List<string>();
        numberOfMatchInterests = 2;
        SetRandomInterest();
        
    }

    public List<string> GetAssignedInterests()
    {
        return assignedInterests;
    }

    private List<Vector4> GenerateRandomTextColours()
    {
        List<Vector4> generatedColours = new List<Vector4>();

        for(int i = 0; i < allInterests.Length; i++)
        {
            Vector4 randomCol = GenerateRandomColour();
            generatedColours.Add(randomCol);
        }
        return generatedColours;
    }

    private Vector4 GenerateRandomColour()
    {
        float r = Random.Range(50, 255)/255.0f;
        float g = Random.Range(50, 255)/255.0f;
        float b = Random.Range(50, 255)/255.0f;
        float a = 1;
        Vector4 col = new Vector4(r, g, b, a);
        return col;
    }

    public void SetRandomInterest()
    {
        int pos = Random.Range(0, allInterests.Length);
        string randomInterest = allInterests[pos];
        for(int i = 0; i < interestTexts.Count; i++)
        {
            while(assignedInterests.Contains(randomInterest))
            {
                pos = Random.Range(0, allInterests.Length);
                randomInterest = allInterests[pos]; 
            }
            interestTexts[i].text = randomInterest;
            interestTexts[i].color = new Color(interestColours[pos].x, interestColours[pos].y, interestColours[pos].z) ;
            assignedInterests.Add(randomInterest);
        }
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
            Debug.Log(lines[i]);
        }
        return lines;
    }

}
