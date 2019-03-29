using System.Collections.Generic;
using UnityEngine;

public class UglyCharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    private float timeBtwnPointerClicks = 1.5f;
    private float clickTimer = 0f;
    private bool hasOnePointerClick = false;
    public static int numberOfRequiredClicks = 2;
    public float infectFriendsRange = 6;
    private List<TextMesh> thoughtTexts;
    public LayerMask whatIsFriends;
    [SerializeField]
    private TextMesh thot;

    private void Start()
    {
        thoughtTexts = new List<TextMesh>();
        thot.gameObject.SetActive(false);
        if(gameObject.tag == "ProudCharacter")
        {
            Debug.Log(whatIsFriends.value);
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] currentCols = Physics2D.OverlapCircleAll(transform.position, infectFriendsRange, whatIsFriends);
        for (int col = 0; col < currentCols.Length; col++)
        {
            CharacterCreation character = currentCols[col].GetComponent<CharacterCreation>();
            if(character.gameObject.tag == "ProudCharacter")
            {
                continue;
            }
            CheckCollider collider = character.GetCollider();
            if (collider != null)
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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, infectFriendsRange);
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

}
