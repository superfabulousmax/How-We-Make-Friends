using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeautifulCharacterController : MonoBehaviour {

    private float scaleDist = 3;
    const double G = 0.667384;
    [SerializeField]
    private LayerMask whatIsAttracted;
    [SerializeField]
    private float attractFriendsRange = 60;
    [SerializeField]
    private TextMesh thot;
    private void Start()
    {
        thot.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Collider2D[] currentCols = Physics2D.OverlapCircleAll(transform.position, attractFriendsRange, whatIsAttracted);
        if(currentCols.Length == 0)
        {
            CharacterCreation character = GetComponent<CharacterCreation>();
            if(character.GetInfectedTimer() > 0)
            {
                character.SetTimer();
            }
        }
        else
        {
            for (int col = 0; col < currentCols.Length; col++)
            {
                //get the offset between the objects
                Vector3 offset = transform.position - currentCols[col].gameObject.transform.position;
                //we're doing 2d physics, so don't want to try and apply z forces!
                offset.z = 0;

                //get the squared distance between the objects
                float magsqr = offset.sqrMagnitude;

                //check distance is more than 0 (to avoid division by 0) and then apply a gravitational force to the object
                //note the force is applied as an acceleration, as acceleration created by gravity is independent of the mass of the object
                Rigidbody2D rigidbody2D = currentCols[col].GetComponent<Rigidbody2D>();
                float attraction = (float) (rigidbody2D.mass * GetComponent<Rigidbody2D>().mass);
                //rigidbody2D.AddForce(attraction * offset.normalized / magsqr);
               
                if (magsqr > 1000f) // 0.0001f)
                {
                    rigidbody2D.AddForce(1000000*attraction * offset.normalized / magsqr);
                }
                else
                {
                    rigidbody2D.velocity = Vector3.zero;
                }
            }
        }
       
    }

    void OnMouseOver()
    {
        Debug.Log("Why does it have ti be thus way");
        thot.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        thot.gameObject.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attractFriendsRange);
    }
}
