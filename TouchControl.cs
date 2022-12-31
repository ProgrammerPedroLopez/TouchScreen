using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    /*
    This script detects the movement of the 
    touchscreen, shot a raycast and moves a 
    cube inside a laberynth. Made for a 
    mobile game
     */

    //Atributtes for touch
    private Vector3 initialPos;
    private Vector3 finalPos;
    private Vector3 result;
    private float x_diference;
    private float y_diference;

    //General
    private bool moving;

    //Atributtes for the lerp movement
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    public float speed;
    private float startTime;
    private float journeyLength;
    private float distCovered;
    private float fractionOfJourney;

    //Atributtes for raycasting
    private RaycastHit raycastHit;
    private float MaxDistanceRay;
    [Tooltip("half of the width/height of the object")]
    public float Offset;

    void Start()
    {
        moving = false;
        MaxDistanceRay = 10f;
        Offset = 0.5f;
    }

    void Update()
    {
        if (moving)
        {
            distCovered = (Time.time - startTime) * speed;
            fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(StartPosition, EndPosition, fractionOfJourney);

            if (transform.position == EndPosition) 
            {
                moving = false;
            }
        }
        else 
        {
            if (Input.touchCount > 0) 
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        initialPos = touch.position;
                        break;

                    case TouchPhase.Moved:
                        break;

                    case TouchPhase.Ended:
                        finalPos = touch.position;
                        SelectOption();
                        break;
                }
            }
        }
        
    }

    void RaycastDetection(Vector3 diection, float offset_x, float offset_y) 
    {
        if (Physics.Raycast(transform.position, diection, out raycastHit, MaxDistanceRay))
        { 
            StartPosition = transform.position;
            EndPosition = raycastHit.point - new Vector3(offset_x, offset_y, 0);
            moving = true;
            startTime = Time.time;
            journeyLength = Vector3.Distance(StartPosition, EndPosition);
        }
    }
    void SelectOption() 
    {
        result = finalPos - initialPos;
        x_diference = result.x;
        y_diference = result.y;

        if (Mathf.Abs(x_diference) > Mathf.Abs(y_diference))
        {
            if (x_diference > 0)
            {
                RaycastDetection(Vector3.right, Offset, 0);
            }
            else
            {
                RaycastDetection(Vector3.left, -Offset, 0);
            }
        }
        else
        {
            if (y_diference > 0)
            {
                RaycastDetection(Vector3.up, 0, Offset);
            }
            else
            {
                RaycastDetection(Vector3.down, 0, -Offset);
            }
        }
    }
}
