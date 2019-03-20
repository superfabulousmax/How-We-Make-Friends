using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovementController : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{

    public float moveSpeed = 30f;
    public float offset = 0.05f;
    public bool following;
    public bool isSelected;
    private float player_width;
    private float player_height;
    private float CLAMP_X;
    private float CLAMP_Y;
    private float CLAMP_OFFSET;
    private Vector4 HIGHLIGHT_COLOUR = new Vector4(1, 1, 0, 255);
    private Vector4 OFF_COLOUR = new Vector4(1, 1, 0, 0); // turn off alpha
    private LevelManager lvlManger;
    public Color originalCol;
    [SerializeField]
    private int destroyUglyPoints = 5;
    private Rigidbody2D rigidbody;
    [SerializeField]
    private ParticleSystem p;
    [SerializeField]
    private Sprite uglyDestroyParticles;

    // Use this for initialization
    void Start()
    {
        following = false;
        isSelected = false;
        player_width = this.GetComponent<Transform>().lossyScale.x;
        player_height = this.GetComponent<Transform>().lossyScale.y;
        offset += 10f;
        CLAMP_X = player_width / Screen.width;
        CLAMP_Y = player_height / Screen.height;
        CLAMP_OFFSET = 0.015f;
        lvlManger = GameObject.Find("GameManager").GetComponent<LevelManager>();
        originalCol = gameObject.GetComponent<SpriteRenderer>().color;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LevelManager.isGameover)
        {
            following = false;
            return;
        }
        //if (Input.GetMouseButtonDown(0) && ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).magnitude <= offset))
        //{
        //    if (following)
        //    {
        //        following = false;
        //    }
        //    else
        //    {
        //        following = true;
        //    }
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (following)
        //    {
        //        following = false;
        //    }
        //}
        if (following)
        {
            isSelected = true;
            //GameObject cacheLastSelected = lvlManger.lastObjectSelected;
            //lvlManger.lastObjectSelected = this.gameObject;
            //if (cacheLastSelected != lvlManger.lastObjectSelected)
            //{

            //    if (cacheLastSelected != null && cacheLastSelected.GetComponent<SpringJoint2D>() == null)
            //    {
            //        cacheLastSelected.GetComponent<SpriteRenderer>().color = cacheLastSelected.GetComponent<CharacterMovementController>().originalCol;
            //    }
            //}
            //heldObject.rigidbody.velocity = (mousePosition - heldObject.transform.position).normalized * desiredMoveSpeed;
            //rigidbody.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * moveSpeed;
            transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), moveSpeed);
            List<GameObject> bobbleHighlight = FindOutline("BobbleOutline");
            if (bobbleHighlight.Count > 0)
            {
                GameObject outline_gameobj = bobbleHighlight[0];
                outline_gameobj.GetComponent<SpriteRenderer>().color = HIGHLIGHT_COLOUR;
                outline_gameobj.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
            }
            else
            {
                // Debug.Log("Bobble Highlight is null");
            }

        }
        else
        {
            isSelected = false;
            List<GameObject> bobbleHighlight = FindOutline("BobbleOutline");
            if (bobbleHighlight.Count > 0)
            {
                GameObject outline_gameobj = bobbleHighlight[0];
                outline_gameobj.GetComponent<SpriteRenderer>().color = OFF_COLOUR;
            }
            else
            {
                // Debug.Log("Bobble Highlight is null");
            }
        }

        // clamp movement to screen boundaries
        var pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, CLAMP_X + CLAMP_OFFSET, 1 - CLAMP_X - CLAMP_OFFSET);
        pos.y = Mathf.Clamp(pos.y, CLAMP_Y + CLAMP_OFFSET, 1 - CLAMP_Y - CLAMP_OFFSET);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public List<GameObject> FindOutline(string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            if (child == this)
            {
                continue;
            }
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
                return taggedGameObjects;
            }
        }
        return taggedGameObjects;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown  " + eventData.pointerPressRaycast.gameObject.tag);

        if (eventData.pointerPressRaycast.gameObject.tag == "StandardCharacter" ||
            eventData.pointerPressRaycast.gameObject.tag == "BeautifulCharacter" ||
            eventData.pointerPressRaycast.gameObject.tag == "UglyCharacter")
        {
            if (following)
            {
                following = false;
            }

            else
            {
                following = true;
            }
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp  " + eventData.pointerPressRaycast.gameObject.tag);

        //if (eventData.pointerPressRaycast.gameObject.tag == "StandardCharacter" || eventData.pointerPressRaycast.gameObject.tag == "BeautifulCharacter")
        //{
        if (following)
        {
            following = false;
        }
        else
        {
            following = true;
        }
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on " + eventData.pointerPressRaycast.gameObject.tag);
        if (eventData.pointerPressRaycast.gameObject.tag == "UglyCharacter")
        {
            if (eventData.clickCount == 2)
            {
                if (eventData.pointerPressRaycast.gameObject.tag == this.gameObject.tag)
                {
                    Debug.Log("Here play and remve");
                    Vector3 pos = transform.position;
                    pos.z = 3;
                    ParticleSystem particleSystem = Instantiate(p, pos, Quaternion.identity);
                    particleSystem.textureSheetAnimation.SetSprite(0, uglyDestroyParticles);

                    particleSystem.Play();
                    float t = particleSystem.main.startLifetime.constantMax + particleSystem.main.duration;
                    Destroy(particleSystem, t);
                    DestroyUgly(destroyUglyPoints);
                }

            }
        }
    }


    private void DestroyUgly(int points)
    {
        Destroy(this.gameObject);
        LevelManager.UpdateScore(points);
    }

    public void MoveTowards(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 10* moveSpeed * Time.deltaTime);
    }

    public void SetVelocity(Vector3 vel)
    {
        rigidbody.velocity = vel;
    }


}
