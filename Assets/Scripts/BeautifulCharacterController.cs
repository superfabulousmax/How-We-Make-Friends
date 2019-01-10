using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeautifulCharacterController : MonoBehaviour {

    PointEffector2D _pointEffector2D;
    Rigidbody2D _rigidbody2D;
    [SerializeField]
    private float minDistance;
    private float scaleDist = 5;

    void Start()
    {
        _pointEffector2D = GetComponent<PointEffector2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        minDistance = Mathf.Max(transform.localScale.x, transform.localScale.y) * scaleDist;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "StandardCharacter" || collision.gameObject.tag == "UglyCharacter")
        {
            if (Vector3.Distance(collision.gameObject.transform.position, transform.position) <= minDistance)
            {
                _rigidbody2D.velocity = Vector3.zero;
                Rigidbody2D colRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if(colRb != null)
                {
                    colRb.velocity = Vector3.zero;
                }
            }
        }
    }
}
