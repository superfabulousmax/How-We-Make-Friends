using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColliderController : MonoBehaviour {

    List<GameObject> collisions;
    GameObject otherCol1;
    GameObject otherCol2;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Line collided with " + col.name);
        bool lineCollided = CheckCollision(this.gameObject, col.gameObject);
    }

    private bool CheckCollision(GameObject col1, GameObject col2)
    {
        if(col1 == otherCol1)
        {
            if(col2 == otherCol2)
            {
                return true;
            }
        }

        else if(col1 == otherCol2)
        {
            if(col2 == otherCol1)
            {
                return true;
            }
        }
        return false;
    }
}
