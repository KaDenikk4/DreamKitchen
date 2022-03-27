using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeInput : MonoBehaviour
{
    private static SwipeInput instance;
    public static SwipeInput Instance // getter and setter for the instance of the class
    {
        get // getter finds an existant swipe input or makes a new one
        {
            if(instance == null)
            {
                instance = FindObjectOfType<SwipeInput>(); 
                if(instance == null)
                {
                    instance = new GameObject("Spawned SwipeInput", typeof(SwipeInput)).GetComponent<SwipeInput>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    [SerializeField] private float deadzone = 100.0f;
    [SerializeField] private float DoubleTapDelta = 0.5f;

    private bool tap, doubleTap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;
    private float lastTap;
    private float sqrDeadzone;

    public bool Tap { get { return tap; } }
    public bool DoubleTap { get { return doubleTap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Start()
    {
        sqrDeadzone = deadzone * deadzone; //that is needed to return the magnitude of the vector (length of vector)
    }

    private void Update()
    {
        // Reseting bools every frame
        tap = doubleTap = swipeLeft = swipeRight = swipeUp = swipeRight = false;
#if UNITY_EDITOR // checking where player launches a game
        UpdateStandalone();
#else
        UpdateMobile();
#endif
    }

    private void UpdateStandalone()
    {
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            Debug.Log("Tap");
            startTouch = Input.mousePosition;
            doubleTap = Time.time - lastTap < DoubleTapDelta;
            if (doubleTap)
                Debug.Log("double tap");
            lastTap = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }

        //reset distance, get the new swipeDelta

        swipeDelta = Vector2.zero;

        //checking the swipe distance
        if (startTouch != Vector2.zero && Input.GetMouseButton(0))
            swipeDelta = (Vector2)Input.mousePosition - startTouch;

        //checking if our delta is beyond deadzone
        if(swipeDelta.sqrMagnitude > sqrDeadzone) // if vector bigger then the deadzone then we confirm swipe
        {
            //we're beyond the deadzone, that's the swipe
            // Checking the direction now
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or right
                if (x < 0)
                {
                    swipeLeft = true;
                    Debug.Log("swipe to the left");
                }
                else
                {
                    swipeRight = true;
                    Debug.Log("swipe to the right");
                }
            }
            else
            {
                //up or down
                if (y < 0)
                {
                    swipeDown = true;
                    Debug.Log("swipe down");
                }
                else
                {
                    swipeUp = true;
                    Debug.Log("swipe up");
                }
            }
            startTouch = swipeDelta = Vector2.zero;
        }
    }

    private void UpdateMobile()
    {
        if (Input.touches[0].phase == TouchPhase.Began)
        {
            tap = true;
            Debug.Log("tap");
            startTouch = Input.mousePosition;
            doubleTap = Time.time - lastTap < DoubleTapDelta;
            if (doubleTap)
                Debug.Log("double tap");
            lastTap = Time.time;
        }
        else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
        {
            startTouch = swipeDelta = Vector2.zero;
        }

        //reset destance, get the new swipeDelta

        swipeDelta = Vector2.zero;

        //checking the swipe distance
        if (startTouch != Vector2.zero && Input.touches.Length != 0)
            swipeDelta = Input.touches[0].position - startTouch;

        //checking if our delta is beyond deadzone
        if (swipeDelta.sqrMagnitude > sqrDeadzone) // if vector bigger then the deadzone then we confirm swipe
        {
            //we're beyond the deadzone, that's the swipe
            // Checking the direction now
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or right
                if (x < 0)
                {
                    swipeLeft = true;
                    Debug.Log("swipe to the left");
                }
                else
                {
                    swipeRight = true;
                    Debug.Log("swipe to the right");
                }
            }
            else
            {
                //up or down
                if (y < 0)
                {
                    swipeDown = true;
                    Debug.Log("swipe down");
                }
                else
                {
                    swipeUp = true;
                    Debug.Log("swipe up");
                }
            }
            startTouch = swipeDelta = Vector2.zero;
        }
    }
}
