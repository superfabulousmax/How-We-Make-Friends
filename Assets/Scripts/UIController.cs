using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text text_score;
    public Text text_gameover;
    public Text text_level;

    public void UpdateTextScore(int score)
    {
        text_score.text = "Score " + score;
    }

    public void UpdateTextLevel(int level)
    {
        text_level.text = "Level " + level;
    }


    public void DisplayGameover(string text)
    {
        text_gameover.text = text;
    }
}
