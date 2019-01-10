using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UglyCharacterBehaviour : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private float timeBtwnPointerClicks = 1.5f;
    private float timer = 0f;
    private float clickTimer = 0f;
    private bool hasOnePointerClick = false;
    private int numberOfRequiredClicks = 2;
    [SerializeField]
    private int numberOfGivenClicks = 0;
    public TextMesh timerText;

    private void Update()
    {
        if(hasOnePointerClick)
        {
            clickTimer += Time.deltaTime;
            if(clickTimer > timeBtwnPointerClicks)
            {
                hasOnePointerClick = false;
                clickTimer = 0;
                numberOfGivenClicks = 0;
            }
            else
            {
                if(numberOfGivenClicks >= numberOfRequiredClicks)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

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
        Debug.Log("OnPointerCLICK");
        if (eventData.pointerPressRaycast.gameObject.tag == this.gameObject.tag)
        {
            Debug.Log("clicked on " + eventData.pointerPressRaycast.gameObject.tag);

            numberOfGivenClicks += 1;
            hasOnePointerClick = true;
        }

        if (eventData.clickCount == 2)
        {
            if (eventData.pointerPressRaycast.gameObject.tag == this.gameObject.tag)
            {
                Destroy(this.gameObject);
            }

        }


    }

}
