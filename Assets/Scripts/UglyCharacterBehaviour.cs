using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UglyCharacterBehaviour : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private float timer = 0f;
    public TextMesh timerText;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "StandardCharacter" || col.tag == "BeautifulCharacter")
        {
            timer += Time.deltaTime;
            timerText.text = "" + timer.ToString("0.00"); //2dp Number;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "StandardCharacter" || col.tag == "BeautifulCharacter")
        {
            timer += Time.deltaTime;
            timerText.text = "" + timer.ToString("0.00"); //2dp Number;
            if (timer > 4)
            {
                timerText.text = "";
                CharacterCreation.CreateCopyCharacter(CharacterCreation.CharacterType.Ugly, col.gameObject, this.gameObject);
                Debug.Log("Infect Character");
                timer = 0;
                LevelManager.UpdateScore(-10);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "StandardCharacter"|| col.tag == "BeautifulCharacter")
        {
            if (timer < 4)
            {
                timerText.text = "";
                timer = 0;
            }
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        Debug.Log("clicked on "+ eventData.pointerPressRaycast.sortingLayer.ToString() + " this tag "+ this.gameObject.tag);
        if (eventData.clickCount == 2)
        {
            if (eventData.pointerPressRaycast.gameObject.tag == this.gameObject.tag)
            {
                Destroy(this.gameObject);
            }
          
        }
    }

}
