using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Blade : MonoBehaviour
{
    [SerializeField]private SwipeInput swipeInput;
    [SerializeField] private float fMinCuttingVelocity;
    private bool bIsCutting;

    private Vector2 previousPosition;

    private Rigidbody2D rb;
    private Camera cam;
    [SerializeField] private GameObject trailPrefab;
    private GameObject currentBladeTrail;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        cam = Camera.main; //camera
        rb = this.GetComponent<Rigidbody2D>(); // rigid body
        circleCollider = this.GetComponent<CircleCollider2D>(); // collider
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if(bIsCutting)
        {
            UpdateBlade();
        }
    }

    void UpdateBlade()
    {
        Vector2 newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPosition;

        float velocity = (newPosition - previousPosition).magnitude * Time.deltaTime; // length of the vector
        if( velocity > fMinCuttingVelocity)
        {
            circleCollider.enabled = true;
        }
        else
        {
            circleCollider.enabled = false;
        }

        previousPosition = newPosition;
    }

    void StartCutting()
    {
        //cutting is enabled
        bIsCutting = true;
        currentBladeTrail = Instantiate(trailPrefab, transform); // trail of the blade
        previousPosition = cam.ScreenToWorldPoint(Input.mousePosition); // setting the previous mouse position
        circleCollider.enabled = false;
    }

    void StopCutting()
    {
        //cutting is disabled
        bIsCutting = false; // not cutting
        currentBladeTrail.transform.SetParent(null);
        Destroy(currentBladeTrail, 2f); // destroying trail
        circleCollider.enabled = false;
    }
}
