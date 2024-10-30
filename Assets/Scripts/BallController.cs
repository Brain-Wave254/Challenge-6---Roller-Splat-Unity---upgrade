using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb; // Rigidbody
    public float speed = 15; // Speed

    public int minSwipeRecognition = 500; // Min swipe

    private bool isTraveling; // Travel state
    private Vector3 travelDirection; // Travel direction

    private Vector2 swipePosLastFrame; // Last swipe
    private Vector2 swipePosCurrentFrame; // Current swipe
    private Vector2 currentSwipe; // Swipe data

    private Vector3 nextCollisionPosition; // Collision point

    private Color solveColor; // Ball color

    private void Start()
    {
        solveColor = Random.ColorHSV(.5f, 1); // Random color
        GetComponent<MeshRenderer>().material.color = solveColor; // Set color
    }

    private void FixedUpdate()
    {
        if (isTraveling)
        {
            rb.velocity = travelDirection * speed; // Move ball
        }

        // Paint ground
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), .05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();

            if (ground && !ground.isColored)
            {
                ground.Colored(solveColor); // Color ground
            }

            i++;
        }

        // Check destination
        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTraveling = false; // Stop traveling
                travelDirection = Vector3.zero; // Reset direction
                nextCollisionPosition = Vector3.zero; // Reset position
            }
        }

        if (isTraveling)
            return; // Exit if traveling

        // Swipe mechanism
        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y); // Current position

            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame; // Calculate swipe

                if (currentSwipe.sqrMagnitude < minSwipeRecognition) // Minimum swipe
                    return; // Exit if small

                currentSwipe.Normalize(); // Normalize direction

                // Up/Down swipe
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back); // Set direction
                }

                // Left/Right swipe
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left); // Set direction
                }
            }

            swipePosLastFrame = swipePosCurrentFrame; // Update last frame
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero; // Reset swipe
            currentSwipe = Vector2.zero; // Reset current swipe
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction; // Set direction

        // Check collision
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point; // Collision point
        }

        isTraveling = true; // Start traveling
    }
}
