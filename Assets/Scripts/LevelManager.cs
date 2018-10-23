using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private UIController uiUpdater;
    private int score = 0;
    void Start()
    {
        uiUpdater = gameObject.GetComponent<UIController>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void UpdateScore(int amount)
    {
        score += amount;
        uiUpdater.UpdateTextScore(score);
    }

    public int GetScore()
    {
        return score;
    }
}
