using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColliderController : MonoBehaviour {

    List<GameObject> collisions;
    GameObject lineCollision;
    GameObject self;

    public void SetSelf(GameObject obj)
    {
        self = obj;
    }

    public void SetCollision(GameObject col)
    {
        lineCollision = col;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if(col.tag == "StandardCharacter" || col.tag == "BeautifulCharacter")
        {
            Debug.Log("Line collided with " + col.tag);
            lineCollision = col.gameObject;
        }
    }

    public bool CheckCollision()
    {
        if(lineCollision == self || lineCollision == null)
        {
            return false;
        }
        return true;
    }
}
