using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour {

    private static UIController uiUpdater;
    private static int score = 0;
    public GameObject lastObjectSelected;
    public string[] allInterests;
    public List<Vector4> interestColours;
    public static bool isGameover = false;
    private void Awake()
    {
        allInterests = ReadInterestsToList();
        interestColours = GenerateRandomTextColours();
    }

    void Start()
    {
        uiUpdater = gameObject.GetComponent<UIController>();
    }

    private List<Vector4> GenerateRandomTextColours()
    {
        List<Vector4> generatedColours = new List<Vector4>();

        for (int i = 0; i < allInterests.Length; i++)
        {
            Vector4 randomCol = GenerateRandomColour();
            generatedColours.Add(randomCol);
        }
        return generatedColours;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            isGameover = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public static void UpdateScore(int amount)
    {
        score += amount;
        uiUpdater.UpdateTextScore(score);
    }

    public static void DisplayGameover(string text)
    {
        uiUpdater.DisplayGameover(text);
    }

    public int GetScore()
    {
        return score;
    }

    private string[] ReadInterestsToList()
    {
        string path = "Assets/Resources/Interests.txt";
        //Read the text from directly from the txt file
        StreamReader reader = new StreamReader(path);
        string[] lines = reader.ReadToEnd().Split('\n');
        reader.Close();
        Debug.Log("Reading in interests to list");
        return lines;
    }

    private Vector4 GenerateRandomColour()
    {
        float r = Random.Range(50, 255) / 255.0f;
        float g = Random.Range(50, 255) / 255.0f;
        float b = Random.Range(50, 255) / 255.0f;
        float a = 1;
        Vector4 col = new Vector4(r, g, b, a);
        return col;
    }

}
