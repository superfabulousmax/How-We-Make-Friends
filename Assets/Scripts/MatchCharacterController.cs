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
    private bool thisObjSelected;
    private bool isMatching;
    private GameObject matchStartObject;
    private GameObject currentRadiusCollision;
    public float meetFriendsRange = 6;
    public LayerMask whatIsFriends;
    private Color myColour;

    void Start ()
    {
        movementScript = GetComponent<CharacterMovementController>();
        connections = new List<GameObject>();
        createdCharacter = GetComponent<CharacterCreation>();
        lvlManger = GameObject.Find("GameManager").GetComponent<LevelManager>();
        numberOfMatches = 0;
        offset = 20f;
        rmouseClick = false;
        isMatching = false;
        thisObjSelected = false;
        myColour = Random.ColorHSV();
        while (myColour.g < 0.3)
        {
            myColour = Random.ColorHSV();
        }
    }

	void Update ()
    {
        if(lvlManger.lastObjectSelected != null && lvlManger.lastObjectSelected.GetComponent< SpringJoint2D>() == null)
        {
           
            lvlManger.lastObjectSelected.GetComponent<SpriteRenderer>().color = Color.yellow;

        }

        if (this.gameObject == lvlManger.lastObjectSelected)
        {

            Collider2D[] currentCols = Physics2D.OverlapCircleAll(transform.position, meetFriendsRange, whatIsFriends);
            for (int col = 0; col < currentCols.Length; col++)
            {

                if (currentCols[col].gameObject.tag == "MatchingRadius")
                {
                    if (this.gameObject == currentCols[col].gameObject.transform.parent.gameObject)
                    {
                        continue;
                    }
                    Transform parentTransform = currentCols[col].gameObject.transform.parent.transform;
                    if (Input.GetMouseButtonDown(1) && ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - parentTransform.position).magnitude <= offset))
                    {
                        if (GetComponent<SpringJoint2D>() == null)
                        {
                            Connect(currentCols[col].gameObject.transform.parent.gameObject);
                        }

                    }
                }
            }
        }

        if (numberOfMatches >= 1 && connections.Count >= 1)
        {
            Destroy(this.gameObject);
            for(int i = 0; i < connections.Count; i ++)
            {
                Destroy(connections[i]);
            }
        }

        for (int i = 0; i < connections.Count; i++)
        {
            if(connections[i] == null)
            {
                continue;
            }
            DrawLine(connections[i].GetComponent<Collider2D>());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "MatchingRadius")
        {
            this.GetComponent<LineRenderer>().enabled = true;
            var line = GetComponent<LineRenderer>();
            line.GetComponent<Renderer>().material.color = Color.white;
            DrawLine(col);
        }
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
        if (col.gameObject.tag == "MatchingRadius")
        {
            DrawLine(col);
        }
    }

    private void Connect(GameObject obj)
    {
        // add connection
        SpringJoint2D connection1 = gameObject.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        connection1.connectedBody = obj.GetComponent<Rigidbody2D>();
        connection1.anchor = new Vector2(0, 0);
        connection1.connectedAnchor = new Vector2(0, 0);
        connection1.distance = 30;
        connection1.enableCollision = true;
        connection1.frequency = 3.0f;
        connection1.autoConfigureDistance = false;
        connection1.dampingRatio = 1;

        SpringJoint2D connection2 = obj.AddComponent(typeof(SpringJoint2D)) as SpringJoint2D;
        connection2.connectedBody = this.GetComponent<Rigidbody2D>();
        connection2.anchor = new Vector2(0, 0);
        connection2.connectedAnchor = new Vector2(0, 0);
        connection2.distance = 30;
        connection2.enableCollision = true;
        connection2.frequency = 3.0f;
        connection2.autoConfigureDistance = false;
        connection2.dampingRatio = 1;

        // Add each other to connections list
        MatchCharacterController otherMatchController = obj.GetComponent<MatchCharacterController>();
        if (!otherMatchController.connections.Contains(this.gameObject)) otherMatchController.connections.Add(this.gameObject);
        if (!this.connections.Contains(obj)) this.connections.Add(obj);

        // change colour to make a group
        this.GetComponent<SpriteRenderer>().color = myColour;
        obj.GetComponent<SpriteRenderer>().color = myColour;
        // Score the match
        CharacterCreation other = obj.GetComponent<CharacterCreation>();
        ScoreMatch(other);
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
                    LevelManager.UpdateScore(score);
                }
            }
        }

        if(matches == 0)
        {
            LevelManager.UpdateScore(-10);
        }
        return score;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meetFriendsRange);
    }

}
