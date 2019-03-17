using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollider : MonoBehaviour {

    public bool isEmpty;
    public float checkCollisonRange = 40;
    public LayerMask whatIsUgly;

    private void Update()
    {
        Collider2D[] currentCols = Physics2D.OverlapCircleAll(transform.position, checkCollisonRange, whatIsUgly);
        isEmpty = true;
        for (int col = 0; col < currentCols.Length; col++)
        {
            isEmpty = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, checkCollisonRange);
    }
}
