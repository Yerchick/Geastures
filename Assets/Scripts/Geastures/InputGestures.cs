using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PDollarGestureRecognizer;
using System;
using System.Text;

public class InputGestures : MonoBehaviour {

    public DataGestures mgData;

    public bool IsGameGoing;

    bool IsTouchInProgress, IsMovementInTouch, WasMovementInTouch;
    Vector2 positionTouchStart;
    bool IsTrailQueuedForDestroy;
    bool IsMouseMovementStarted;
    bool IsGestureRecognizingNeeded;
    bool IsStartTrailSpawned;

	List<Point> currentGesturePoints;
    GameObject trail;

    public GameObject lineRenderer;
    int currentVertice = 0;
  //  List<Vector3> gesturePoints;

	void Start () {
        IsTouchInProgress = IsTrailQueuedForDestroy = IsMouseMovementStarted = IsStartTrailSpawned = false;
	}

	// Update is called once per frame
	void Update ()
    {
        Vector2 cursorPosition = new Vector2();
        
		if ((Input.touchCount > 0))
		{
			Touch touch = Input.GetTouch(0);
			cursorPosition = touch.position;
			{
				switch (touch.phase)
				{
				case TouchPhase.Began:
					StartGesture(cursorPosition);
					break;
				case TouchPhase.Stationary:
					IsMovementInTouch = false;
					break;
				case TouchPhase.Moved:
					MoveGesture(cursorPosition);
					break;
				case TouchPhase.Ended:
					FinishGesture(cursorPosition);
					break;
				case TouchPhase.Canceled:
					IsTouchInProgress = false;
					break;
				}
			}
		}
		else if (Input.GetMouseButton(0))
		{
			cursorPosition = Input.mousePosition;
			if (Input.GetMouseButtonDown(0)) //touch begin
			{
				if (!IsMouseMovementStarted)
				{
					IsMouseMovementStarted = true;
					StartGesture(cursorPosition);
				}
			}
			else //touchInProgress
			{
				if ((Mathf.Abs(Input.GetAxis("Mouse X")) > 0) || (Mathf.Abs(Input.GetAxis("Mouse Y")) > 0))//mouse moved
					MoveGesture(cursorPosition);
				else //stationary
					IsMovementInTouch = false;
			}
		}
		else //no input
		{
			if (IsMouseMovementStarted)
			{
				FinishGesture(Vector3.zero);
				IsMouseMovementStarted = false;
			}
		}


        switch (IsGameGoing)
        {
            
            case false:
                break;
            case true:
                if (IsTouchInProgress)
                {
                    if ((!WasMovementInTouch)&&(!IsStartTrailSpawned)) //begin
                    {
                        Vector3 pos = Camera.main.ScreenToWorldPoint(cursorPosition);
                        pos.z = 0; // Make sure the trail is visible
                        if (trail != null) 
                            Destroy(trail);
                        trail = (GameObject)Instantiate(lineRenderer);
                       // trail.GetComponent<LineRenderer>().SetPosition(0, pos);

                    //    gesturePoints.Add(pos);

                        IsStartTrailSpawned = true;
                    }
                    else
                        if (IsMovementInTouch) //move
                        {
                            currentVertice++;

                            Vector3 position = Camera.main.ScreenToWorldPoint(cursorPosition);

                            position.z = -100; // Make sure the trail is visible

                       //     gesturePoints.Add(position);

                           // LineRenderer lineRenderer = trail.GetComponent<LineRenderer>();
                            //lineRenderer.SetVertexCount(currentVertice);


                           /* for (int i = 0; i < currentVertice; i++)
                            {
                              //  lineRenderer.SetPosition(i, gesturePoints[i]);
                            }*/
                        }
                }
                else //end
                {
                    if ((trail != null)&&(!IsTrailQueuedForDestroy))
                    {
                        IsTrailQueuedForDestroy = true;
                        Destroy(trail.gameObject);
                   //     gesturePoints = new List<Vector3>();
                        currentVertice = 0;

                        IsStartTrailSpawned = false;
                        if (currentGesturePoints.Count > 2) //need more than 2 points for gesture recognition
                        {
                            mgData.RecognizeGesture(currentGesturePoints);
                            IsGestureRecognizingNeeded = false;
                        }
                    }
                }
                break;

        }
    }

	void StartGesture(Vector3 pos)
	{
		positionTouchStart = pos;
		IsTouchInProgress = true;
		IsMovementInTouch = WasMovementInTouch = false;
		IsTrailQueuedForDestroy = false;
		IsGestureRecognizingNeeded = false;
		currentGesturePoints = new List<Point>();
		currentGesturePoints.Add(new Point(pos.x, -pos.y, 1));
	}

	void MoveGesture(Vector3 pos)
	{
		IsMovementInTouch = WasMovementInTouch = true;
		if(currentGesturePoints == null){
		}else{
			currentGesturePoints.Add(new Point(pos.x, -pos.y, 1));
		}

	}

	void FinishGesture(Vector3 pos)
	{
		if (IsTouchInProgress)
		{
			IsTouchInProgress = false;
			if (pos != Vector3.zero)
				currentGesturePoints.Add(new Point(pos.x, -pos.y, 1));
		}
	}
}
