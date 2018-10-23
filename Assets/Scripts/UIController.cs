using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public Text text_score;
	public void UpdateTextScore(int score)
    {
        text_score.text = "Score " + score;
    }
}
