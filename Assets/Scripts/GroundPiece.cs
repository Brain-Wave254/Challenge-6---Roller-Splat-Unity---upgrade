using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPiece : MonoBehaviour
{
    public bool isColored = false; // Color state

    public void Colored(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color; // Set color
        isColored = true; // Update state

        FindObjectOfType<GameManager>().CheckComplete(); // Check completion
    }
}
