using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UglyCharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    private float timeBtwnPointerClicks = 1.5f;
    private float clickTimer = 0f;
    private bool hasOnePointerClick = false;
    private int numberOfRequiredClicks = 2;
    [SerializeField]
    private int numberOfGivenClicks = 0;
    public float infectFriendsRange = 6;
    public LayerMask whatIsFriends;

    private void FixedUpdate()
    {
        Collider2D[] currentCols = Physics2D.OverlapCircleAll(transform.position, infectFriendsRange, whatIsFriends);
        for (int col = 0; col < currentCols.Length; col++)
        {
            CharacterCreation character = currentCols[col].GetComponent<CharacterCreation>();
            CheckCollider collider = character.GetCollider();
            if(collider != null)
            {
                collider.isEmpty = false;
            }
            character.UpdateInfectionTimer();
            bool useColTimer = true;
            character.SetTimer(0, useColTimer);
            if (character.IsInfected())
            {
                CharacterCreation.CreateCopyCharacter(CharacterCreation.CharacterType.Ugly, currentCols[col].gameObject, this.gameObject);
                LevelManager.UpdateScore(-10);
                character.SetTimer();
            }
        }

        if (hasOnePointerClick)
        {
            clickTimer += Time.deltaTime;
            if(clickTimer > timeBtwnPointerClicks)
            {
                hasOnePointerClick = false;
                clickTimer = 0;
                numberOfGivenClicks = 0;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, infectFriendsRange);
    }

}
