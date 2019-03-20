using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeautifulCharacterController : MonoBehaviour {

    PointEffector2D _pointEffector2D;
    Rigidbody2D _rigidbody2D;
    [SerializeField]
    private float minDistance;
    private float scaleDist = 3;
    const double G = 0.667384;
    [SerializeField]
    private LayerMask whatIsAttracted;
    [SerializeField]
    private float attractFriendsRange = 60;
    void Start()
    {
        _pointEffector2D = GetComponent<PointEffector2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        minDistance = 2*Mathf.Max(transform.lossyScale.x, transform.lossyScale.y) * scaleDist;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attractFriendsRange);
    }
}
