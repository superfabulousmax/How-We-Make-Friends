using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdDetector : MonoBehaviour {
    public float crowdDectorRange = 100;
    public LayerMask whatIsCrowd;
    public int MAX_CROWD_NUMBER = 5;
    public int WARNING_CROWD_NUMBER = 25;
    private bool hasWarnedPlayer = false;
    // Use this for initialization
    void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        Collider2D[] currentCols = Physics2D.OverlapCircleAll(transform.position, crowdDectorRange, whatIsCrowd);
        if (currentCols.Length >= MAX_CROWD_NUMBER)
        {
            Time.timeScale = 0.05f;
            LevelManager.isGameover = true;

            LevelManager.DisplayGameover("Game Over!\nOh no, too crowded! " +
                "\nR to restart");
            
        }

        if (!hasWarnedPlayer && currentCols.Length >= WARNING_CROWD_NUMBER)
        {
            hasWarnedPlayer = true;
            DisplayOvercrowdedWarning();
        }

        if(currentCols.Length < WARNING_CROWD_NUMBER)
        {
            hasWarnedPlayer = false;
        }
    }

    void DisplayOvercrowdedWarning()
    {
        LevelManager.DisplayGameover("Becoming too crowded!");
        StartCoroutine(WaitAndRemoveMessage());
    }

    IEnumerator WaitAndRemoveMessage()
    {
        yield return new WaitForSeconds(4f);
        LevelManager.DisplayGameover("");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, crowdDectorRange);
    }
}
