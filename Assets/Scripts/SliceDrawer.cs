using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceDrawer : MonoBehaviour {

    public Camera cam;
    public float lineWidth = 1;
    public Material lineMaterial;
    private bool canDrawLine;
    // Vector3? means make vector nullable 
    // usually vector is not nullable and must have a value
    public Vector3? lineStartPoint = null;
    private Vector3? lineEndPoint = null;
    public float depth = 5;
    private float waitTime;
    private GameObject collision1;
    private GameObject collision2;
    private GameObject line;
    private LineRenderer lineRenderer;
    private bool hasLineColliderAdded;

    public void SetCanDrawLine(bool v)
    {
        canDrawLine = v;
        Debug.Log("set can draw line"+canDrawLine);
    }

    void Start()
    {
        waitTime = 2;
        canDrawLine = false;
        hasLineColliderAdded = false;
        line = new GameObject();
        line.transform.parent = this.transform;
        lineRenderer = line.AddComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if(canDrawLine)
        {
            lineEndPoint = GetMouseCameraPoint();
            DrawFromPlayer();
            var start = new Vector3(lineStartPoint.Value.x, lineStartPoint.Value.y, lineStartPoint.Value.z);
            var end = new Vector3(lineEndPoint.Value.x, lineEndPoint.Value.y, lineEndPoint.Value.z);
            if (!hasLineColliderAdded)
            {
                AddColliderToLine(line, lineRenderer, start, end);
            }
            else
            {
                UpdateLineCollider(line, lineRenderer, start, end);
            }
        }
    }

    public void SetCollision(GameObject col1, GameObject col2)
    {
        collision1 = col1;
        collision2 = col2;
    }

    public void DrawLine()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineStartPoint = GetMouseCameraPoint();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (lineStartPoint == null)
            {
                return;
            }
            var lineEndPoint = GetMouseCameraPoint();
            var start = new Vector3(lineStartPoint.Value.x, lineStartPoint.Value.y, lineStartPoint.Value.z);
            var gameObject = new GameObject();
            gameObject.transform.parent = this.transform;

            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = lineMaterial;
            lineRenderer.SetPositions(new Vector3[] { start, lineEndPoint });
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineStartPoint = null;
            AddColliderToLine(gameObject, lineRenderer, start, lineEndPoint);
            StartCoroutine(WaitThenDeleteLine(waitTime, gameObject));
        }

    }

    public void DrawFromPlayer()
    {
        Debug.Log("Drawing line from player to mouse");
        var end = new Vector3(lineEndPoint.Value.x, lineEndPoint.Value.y, lineEndPoint.Value.z);
        var start = new Vector3(lineStartPoint.Value.x, lineStartPoint.Value.y, lineStartPoint.Value.z);
        lineRenderer.material = lineMaterial;
        lineRenderer.SetPositions(new Vector3[] { start, end });
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        //AddColliderToLine(line, lineRenderer, start, end);
    }

    private void AddColliderToLine(GameObject lineObject, LineRenderer line, Vector3 startPoint, Vector3 endPoint)
    {
        //create the collider for the line
        if(lineStartPoint == null)
        {
            return;
        }
        if(lineEndPoint == null)
        {
            return;
        }

        lineObject.AddComponent<BoxCollider2D>();
        lineObject.gameObject.AddComponent<LineColliderController>();
        lineObject.GetComponent<BoxCollider2D>().isTrigger = true;
        UpdateLineCollider(lineObject, lineRenderer, startPoint, endPoint);
        hasLineColliderAdded = true;


    }

    private void UpdateLineCollider(GameObject lineObject, LineRenderer line, Vector3 startPoint, Vector3 endPoint)
    {
        //set the collider as a child of your line
        lineObject.transform.parent = line.transform;
        // get width of collider from line 
        float lineWidth = line.endWidth;
        // get the length of the line using the Distance method
        float lineLength = Vector3.Distance(startPoint, endPoint);
        // size of collider is set where X is length of line, Y is width of line
        //z will be how far the collider reaches to the sky
        lineObject.GetComponent<BoxCollider2D>().size = new Vector3(lineLength, lineWidth, 1f);
        // get the midPoint
        Vector3 midPoint = (startPoint + endPoint) / 2;
        // move the created collider to the midPoint
        lineObject.transform.position = midPoint;


        //heres the beef of the function, Mathf.Atan2 wants the slope, be careful however because it wants it in a weird form
        //it will divide for you so just plug in your (y2-y1),(x2,x1)
        float angle = Mathf.Atan2((endPoint.y - startPoint.y), (endPoint.x - startPoint.x));

        // angle now holds our answer but it's in radians, we want degrees
        // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
        angle *= Mathf.Rad2Deg;

        //were interested in the inverse so multiply by -1
        angle *= -1;
        // now apply the rotation to the collider's transform, carful where you put the angle variable
        // in 3d space you don't wan't to rotate on your y axis
        lineObject.transform.Rotate(0, angle, 0);
    }

    private Vector3 GetMouseCameraPoint()
    {
        var ray =  cam.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * depth;
    }

    private IEnumerator WaitThenDeleteLine(float waitTime, GameObject lineObject)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Destroy(lineObject);
        }
    }
}
