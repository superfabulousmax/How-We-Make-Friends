using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendHome : MonoBehaviour {

    private 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		 
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Send Home Trigger Fired");
        MatchCharacterController matchInterface = col.gameObject.GetComponent<MatchCharacterController>();
        if(matchInterface != null)
        {
            int matches = matchInterface.numberOfMatches;
            if (matches >= 2)
            {
                for (int i = 0; i < matchInterface.connections.Count; i++)
                {
                    Destroy(matchInterface.connections[i].gameObject);
                }
                Destroy(this.gameObject);
            }
           
        }
    }
}
