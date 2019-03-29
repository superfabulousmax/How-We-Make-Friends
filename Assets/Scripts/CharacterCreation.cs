using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {

    private LevelManager lvlManager;
    private List<string> assignedInterests;
    private List<TextMesh> interestTexts;
    public int numberOfMatchInterests;
    public GameObject interestsContainer;
    public enum CharacterType { Standard, Beautiful, Ugly };
    public CharacterType myType;
    private float myInfectionTimer = 0;
    [SerializeField]
    private TextMesh timerText;
    [SerializeField]
    private CheckCollider collider;
    private Animator animator;
    private float blinkTimer = 0;
    private float blinkTimeStep;
    [HideInInspector]
    void Start()
    {
      
        lvlManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
        interestTexts = new List<TextMesh>();
        numberOfMatchInterests = LevelManager.currentLevel;
        for (int i = 0; i < interestsContainer.transform.childCount; i++)
        {
            if (i < numberOfMatchInterests)
            {
                interestsContainer.transform.GetChild(i).gameObject.SetActive(true);
                interestTexts.Add(interestsContainer.transform.GetComponentsInChildren<TextMesh>()[i]);
            }
                
            else interestsContainer.transform.GetChild(i).gameObject.SetActive(false);

        }
        animator = GetComponent<Animator>();
        blinkTimeStep = Random.Range(5, 10);
        assignedInterests = new List<string>();
        SetRandomInterest(0);
    }

    private void Update()
    {
        
        if (numberOfMatchInterests < LevelManager.currentLevel)
        {
            for (int i = numberOfMatchInterests; i < LevelManager.currentLevel; i++)
            {
                
                interestsContainer.transform.GetChild(i).gameObject.SetActive(true);
                interestTexts.Add(interestsContainer.transform.GetComponentsInChildren<TextMesh>()[i]);                
                SetRandomInterest(numberOfMatchInterests);
            }

            numberOfMatchInterests = LevelManager.currentLevel;
            //Animator animator = GetComponent<Animator>();
            //animator.Play()
        }
        if(collider!=null)
        {
            if (collider.isEmpty && myInfectionTimer > 0)
            {
                SetTimer();
            }
        }
        blinkTimer += Time.deltaTime;
        if(myType == CharacterType.Standard)
        {
            if(blinkTimer >= blinkTimeStep)
            {
                animator.SetBool("isBlinking", true);
                StartCoroutine(BlinkThenStop());
                blinkTimer = 0;
                blinkTimeStep = Random.Range(3, 10);
            }
        }
            
    }

    IEnumerator BlinkThenStop()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isBlinking", false);
    }
    public void UpdateInfectionTimer()
    {
        myInfectionTimer += Time.deltaTime;
    }

    public List<string> GetAssignedInterests()
    {
        return assignedInterests;
    }

    public void SetRandomInterest(int start)
    {
        int pos = Random.Range(0, lvlManager.allInterests.Length);       
        string randomInterest = lvlManager.allInterests[pos];
        
        for(int i = start; i < interestTexts.Count; i++)
        {
            while(assignedInterests.Contains(randomInterest))
            {
                pos = Random.Range(0, lvlManager.allInterests.Length);
                randomInterest = lvlManager.allInterests[pos]; 
            }

            interestTexts[i].text = randomInterest;
            interestTexts[i].color = new Color(lvlManager.interestColours[pos].x, lvlManager.interestColours[pos].y, lvlManager.interestColours[pos].z) ;
            assignedInterests.Add(randomInterest);
        }
    }

    public static CharacterType GetRandomType()
    {
        int enumLength = CharacterType.GetNames(typeof(CharacterType)).Length;
        int random = Random.Range(0, 100);
        if (random < 60)
        {
            return CharacterType.Standard;
        }

        if (random > 60 & random < 70)
        {
            return CharacterType.Beautiful;
        }

        else
        {
            return CharacterType.Ugly;
        }
    }

    public static void CreateCopyCharacter(CharacterType type, GameObject oldCharacter, GameObject infector)
    {
        GameObject newCharacter = Instantiate(infector, oldCharacter.transform.position, oldCharacter.transform.rotation);
        newCharacter.GetComponent<CharacterCreation>().SetInterests(oldCharacter.GetComponent<CharacterCreation>().GetInterestTexts());
        newCharacter.GetComponent<SpriteRenderer>().color = infector.GetComponent<SpriteRenderer>().color;
        Destroy(oldCharacter);

    }

    public void SetInterests(List<TextMesh> texts)
    {
        interestTexts = texts;
    }

    public List<TextMesh> GetInterestTexts()
    {
        return interestTexts;
    }

    public void SetTimer(float time = 0, bool useMyTimer = false)
    {
        if (timerText == null)
        {
            return;
        }

        if(useMyTimer)
        {
            timerText.text = myInfectionTimer.ToString("0.00");
        }
        else
        {
            timerText.text = "";
            myInfectionTimer = time;
        }
        
    }

    public float GetInfectedTimer()
    {
        return myInfectionTimer;
    }

    public bool IsInfected()
    {
        return myInfectionTimer > 4;
    }

    public CheckCollider GetCollider()
    {
        return collider;
    }
}

