using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCharacterController : MonoBehaviour {

    public CharacterMovementController movementScript;
    public List<GameObject> connections;
    public SliceDrawer lineDrawer;
    private CharacterCreation createdCharacter;
    [HideInInspector]
    public int numberOfMatches;
    public LevelManager lvlManger;
    private float offset;
    private bool rmouseClick;

    void Start ()
    {
        movementScript = GetComponent<CharacterMovementController>();
        connections = new List<GameObject>();
        lineDrawer = GameObject.FindGameObjectWithTag("LineDrawer").GetComponent<SliceDrawer>();
 
        createdCharacter = GetComponent<CharacterCreation>();
        lvlManger = GameObject.Find("GameManager").GetComponent<LevelManager>();
        numberOfMatches = 0;
        offset = 0.05f;
        rmouseClick = false;
    }
	void Update ()
    {
        if(numberOfMatches >= 1 && connections.Count >= 1)
        {
            Destroy(this.gameObject);
            for(int i = 0; i < connections.Count; i ++)
            {
                Destroy(connections[i]);
            }
        }

        CheckForMatchInput();
    }

    bool CheckForMatchInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Clicked Meeee!");
            rmouseClick = true;
            return rmouseClick;
        }
        rmouseClick = false;
        return rmouseClick;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.tag + " : " + gameObject.name + " : " + Time.time);
        // TODO remove
        if(col.gameObject.tag == "MatchingRadius")
        {
            
        }
        
        this.GetComponent<LineRenderer>().enabled = true;
        var line = GetComponent<LineRenderer>();
        line.GetComponent<Renderer>().material.color = Color.white;
        DrawLine(col);
    }

    private void DrawLine(Collider2D col)
    {
        var lr = this.gameObject.GetComponent<LineRenderer>();
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, col.transform.position);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.GetComponent<LineRenderer>().enabled = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        
        var lr = this.GetComponent<LineRenderer>();
        //lr.GetComponent<Renderer>().material.color = Color.yellow;
        //lr.SetPosition(0, this.transform.position);
        //lr.SetPosition(1, col.transform.position);
        //Camera.main.ScreenToWorldPoint(Input.mousePosition) 
        

        if (col.gameObject.tag == "MatchingRadius")
        {
            CharacterMovementController mvScript = col.gameObject.transform.parent.GetComponent<CharacterMovementController>();
            if(mvScript.isSelected)
            {
                mvScript.following = false;
                lineDrawer.SetCanDrawLine(true);
                lineDrawer.lineStartPoint = col.gameObject.transform.parent.transform.position;

            }
            
            lineDrawer.SetCollision(this.gameObject, col.gameObject.transform.parent.gameObject);

            if (GetComponent<SpringJoint2D>() == null && false)
            {
                SpringJoint2D connection = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
                connection.connectedBody = col.transform.parent.GetComponent<Rigidbody2D>();
                connection.anchor = new Vector2(0, 0);
                connection.connectedAnchor = new Vector2(0, 0);
                connection.distance = 30;
                connection.enableCollision = true;
                connection.frequency = 3.0f;
                connection.autoConfigureDistance = false;
                connection.dampingRatio = 1;
    
                // Add each other to connections list
                MatchCharacterController otherMatchController = col.gameObject.transform.parent.GetComponent<MatchCharacterController>();
                if (!otherMatchController.connections.Contains(this.gameObject)) otherMatchController.connections.Add(this.gameObject);
                if(!this.connections.Contains(col.transform.parent.gameObject)) this.connections.Add(col.transform.parent.gameObject);
                Debug.Log(this.gameObject.name+" connection added to" + col.gameObject.transform.parent.name);

                // Score the match
                CharacterCreation other = col.gameObject.transform.parent.GetComponent<CharacterCreation>();
                ScoreMatch(other);
            }
            
        }

    }

    private int ScoreMatch(CharacterCreation other)
    {
        int matches = 0;
        int score = 0;
        int numberOfInterests = createdCharacter.numberOfMatchInterests;
        for (int i = 0; i < numberOfInterests; i++)
        {
            for (int j = 0; j < numberOfInterests; j++)
            {
                if(createdCharacter.GetAssignedInterests()[i] == other.GetAssignedInterests()[j])
                {
                    matches += 1;
                    this.numberOfMatches += 1;
                    other.gameObject.GetComponent<MatchCharacterController>().numberOfMatches += 1;
                    score = 10;
                    lvlManger.UpdateScore(score);
                }
            }
        }
        Debug.Log("score: " + score);
        if(matches == 0)
        {
            lvlManger.UpdateScore(-10);
        }
        return score;
    }
}
