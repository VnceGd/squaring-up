using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;

    // Player's y offset relative to the camera
    public float playerOffsetY = -3;
    // Speed at which the camera moves to its target position
    public float camSpeed = 1;

    void Start()
    {
        // Assign the player gameobject on start
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // Set target position based on player postion and offset
        Vector3 targetPosition = player.transform.position + (Vector3.up * playerOffsetY);
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, -10);

        // Move camera position smoothly towards target position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, camSpeed);
        transform.position = smoothedPosition;
    }
}
