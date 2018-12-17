using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text text_score;
    public Text text_gameover;

    public void UpdateTextScore(int score)
    {
        text_score.text = "Score " + score;
    }

    public void DisplayGameover(string text)
    {
        text_gameover.text = text;
    }
}
