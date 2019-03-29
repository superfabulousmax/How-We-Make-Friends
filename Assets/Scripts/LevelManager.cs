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
    public static int currentLevel = 1;
    private static int nextLevel = 2;
    private static int nextLevelScoreRequirement = 100;
    private static int levelIncrement = 100;
    private static int totalNumberOfLevels = 4;
    private int minScore = -100;
    [SerializeField]
    private TextAsset textAsset;
    private void Awake()
    {
        allInterests = ReadInterestsToList();
    }

    void Start()
    {
       
        interestColours = GenerateRandomTextColours();
        uiUpdater = gameObject.GetComponent<UIController>();
        uiUpdater.UpdateTextLevel(currentLevel);
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if(score >= nextLevelScoreRequirement)
        {
            IncreaseLevel();
        }
        if(!isGameover && score <= minScore)
        {
            Time.timeScale = 0.05f;
            isGameover = true;
            DisplayGameover("Game Over!\nScore too low " +
                "\nR to restart");
        }
    }

    public static void UpdateScore(int amount)
    {
        score += amount;
        uiUpdater.UpdateTextScore(score);
    }

    public static void IncreaseLevel()
    {
        if (currentLevel < totalNumberOfLevels)
        {
            currentLevel += 1;
            nextLevel += 1;
            nextLevelScoreRequirement += levelIncrement;
        }
        uiUpdater.UpdateTextLevel(currentLevel);
    }

    public static void DisplayGameover(string text)
    {
        if(uiUpdater == null)
        {
            Debug.Log("uiUpdater is null!!!!");
        }
        Debug.Log("Game over: " + text);
        uiUpdater.DisplayGameover(text);
    }

    public int GetScore()
    {
        return score;
    }

    private string[] ReadInterestsToList()
    {
        string[] lines = textAsset.text.Split('\n');
   
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

    private void Reload()
    {
        isGameover = false;
        Time.timeScale = 1;
        DisplayGameover("");
        currentLevel = 1;
        nextLevel = 2;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
