using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour {

    public List<Transform> spawnPoints;
    
    public int thrust;
    public int spawnTimeUpperLimit;
    public int spawnTimeLowerLimit;
    private float timer;
    private float moveTimer;
    public List<GameObject> charactersToSpawn;
    void Awake()
    {
        float waitTime = Random.Range(2, 10);
        timer = Time.time + waitTime;
    }

    void Start ()
    {
        thrust = 4;
        spawnTimeLowerLimit = 2;
        spawnTimeUpperLimit = 1;
    }

	void Update ()
    {        
        if (timer < Time.time)
        {
            //This checks wether real time has caught up to the timer
            SpawnCharacter();
        }
    }

    void SpawnCharacter()
    {
        CharacterCreation.CharacterType type = CharacterCreation.GetRandomType();
        int spawnPoint = Random.Range(0, spawnPoints.Count);
        int characterPos = 0; // standard is 0

        if(type == CharacterCreation.CharacterType.Beautiful)
        {
            characterPos = 1;
        }

        if (type == CharacterCreation.CharacterType.Ugly)
        {
            characterPos = 2;
        }

        GameObject newCharacter = Instantiate(charactersToSpawn[characterPos], spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation); //This spawns the character
        while (moveTimer <= 10)
        {
            moveTimer += Time.deltaTime;
            MoveAwayFromSpawnPoint(newCharacter, spawnPoint);
        }

        float waitTime = Random.Range(spawnTimeLowerLimit, spawnTimeUpperLimit);
        timer = Time.time + waitTime;
        moveTimer = 0.0f;
    }

    void MoveAwayFromSpawnPoint(GameObject character, int direction)
    {
        if(direction == 0)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector3(10, -10, 0) * thrust);
        }
        else if(direction == 1)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector3(10, 10, 0) * thrust);
        }
        else if(direction == 2)
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector3(-10, 10, 0) * thrust);
        }
        else
        {
            character.GetComponent<Rigidbody2D>().AddForce(new Vector3(-10, -10, 0) * thrust);
        }
    }
}
