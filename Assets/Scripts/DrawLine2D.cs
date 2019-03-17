using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine2D : MonoBehaviour
{

    [SerializeField]
    protected LineRenderer m_LineRenderer;
    [SerializeField]
    protected bool m_AddCollider = false;
    [SerializeField]
    protected EdgeCollider2D m_EdgeCollider2D;
    [SerializeField]
    protected Camera m_Camera;
    protected List<Vector2> m_Points;
    private enum MouseSettings { Left, Middle, Right};
    [SerializeField]
    private MouseSettings mouse;
    private int mouseButton = 0;
    private int currentPos = 0;
    [SerializeField]
    private LayerMask whatIsObject;
    private List< GameObject> hitObjects;

    public virtual LineRenderer lineRenderer
    {
        get
        {
            return m_LineRenderer;
        }
    }

    public virtual bool addCollider
    {
        get
        {
            return m_AddCollider;
        }
    }

    public virtual EdgeCollider2D edgeCollider2D
    {
        get
        {
            return m_EdgeCollider2D;
        }
    }

    public virtual List<Vector2> points
    {
        get
        {
            return m_Points;
        }
    }

    protected virtual void Awake()
    {
        hitObjects = new List<GameObject>();
        if (mouse == MouseSettings.Left)
        {
            mouseButton = 0;
        }
        if(mouse == MouseSettings.Middle)
        {
            mouseButton = 2;
        }
        else
        {
            mouseButton = 1;
        }

        if (m_LineRenderer == null)
        {
            Debug.LogWarning("DrawLine: Line Renderer not assigned, Adding and Using default Line Renderer.");
            CreateDefaultLineRenderer();
        }
        if (m_EdgeCollider2D == null && m_AddCollider)
        {
            Debug.LogWarning("DrawLine: Edge Collider 2D not assigned, Adding and Using default Edge Collider 2D.");
            CreateDefaultEdgeCollider2D();
        }
        if (m_Camera == null)
        {
            m_Camera = Camera.main;
        }
        m_Points = new List<Vector2>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(mouseButton))
        {
            Reset();
        }
        if (Input.GetMouseButton(mouseButton))
        {
            Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            if (!m_Points.Contains(mousePosition))
            {
                m_Points.Add(mousePosition);
                m_LineRenderer.positionCount = m_Points.Count;
                m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, mousePosition);
                if (m_EdgeCollider2D != null && m_AddCollider && m_Points.Count > 1)
                {
                    m_EdgeCollider2D.isTrigger = true;
                    m_EdgeCollider2D.points = m_Points.ToArray();
                    
                }

                if (currentPos < m_EdgeCollider2D.points.Length - 1)
                {
                    RaycastHit2D[] hits = Physics2D.LinecastAll(m_EdgeCollider2D.points[currentPos], m_EdgeCollider2D.points[currentPos + 1], whatIsObject);
                    currentPos++;
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (!hitObjects.Contains(hit.transform.gameObject))
                        {
                            hitObjects.Add(hit.transform.gameObject);
                        }
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(mouseButton))
        {
            if (hitObjects.Count >= 2)
            {
                Match();
            }
        }
    }

    private void Match()
    {
        foreach (GameObject hit in hitObjects)
        {
            if (hit == this || hit == null) return;
            Debug.Log(hit.transform.gameObject.name);

            //Destroy(hit);
        }

        for(int i = 0; i < hitObjects.Count - 1; i++)
        {
            GameObject hit1 = hitObjects[i];
            GameObject hit2 = hitObjects[i + 1];
            MatchCharacterController match1 =  hit1.GetComponent<MatchCharacterController>();
            if(match1!=null)
            {
                match1.Connect(hit2);
            }
        }
    }

    protected virtual void Reset()
    {
        if (m_LineRenderer != null)
        {
            m_LineRenderer.positionCount = 0;
        }
        if (m_Points != null)
        {
            m_Points.Clear();
        }
        if (m_EdgeCollider2D != null && m_AddCollider)
        {
            m_EdgeCollider2D.Reset();
        }
        if(hitObjects != null)
        {
            hitObjects.Clear();
        }
        currentPos = 0;
    }

    protected virtual void CreateDefaultLineRenderer()
    {
        m_LineRenderer = gameObject.AddComponent<LineRenderer>();
        m_LineRenderer.positionCount = 0;
        m_LineRenderer.material = new Material(Shader.Find("Transparent/Diffuse"));
        m_LineRenderer.startColor = Color.white;
        m_LineRenderer.endColor = Color.white;
        m_LineRenderer.startWidth = 0.2f;
        m_LineRenderer.endWidth = 0.2f;
        m_LineRenderer.useWorldSpace = true;
    }

    protected virtual void CreateDefaultEdgeCollider2D()
    {
        m_EdgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
    }
}