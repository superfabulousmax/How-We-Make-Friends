using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {

    private LevelManager lvlManager;
    private List<string> assignedInterests;
    private List<TextMesh> interestTexts;
    public int numberOfMatchInterests = 2;
    public GameObject interestsContainer;
    public enum CharacterType { Standard, Beautiful, Ugly };
    public CharacterType myType;
   

    void Start()
    {
        lvlManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
        interestTexts = new List<TextMesh>();
        interestTexts.Capacity = numberOfMatchInterests;

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

    public void SetRandomInterest()
    {
        int pos = Random.Range(0, lvlManager.allInterests.Length);       
        string randomInterest = lvlManager.allInterests[pos];
        
        for(int i = 0; i < interestTexts.Count; i++)
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
}
