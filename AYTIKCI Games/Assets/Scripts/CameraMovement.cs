using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float zoomSpeed = 0.1f;      // Speed of zooming in/out
    public float rotationSpeed = 0.2f;  // Speed of rotating the camera
    public float moveSpeed = 0.1f;      // Speed of moving the camera (horizontal only)

    private Vector3 lastTouchPos;
    private float initialTouchDistance;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            // Single touch for moving the camera horizontally only
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDelta = touch.deltaPosition;

                // Only move horizontally (X-axis), so set Y and Z to 0
                float moveX = touchDelta.x * moveSpeed;
                transform.Translate(-moveX, 0, 0);
            }
        }
        else if (Input.touchCount == 2)
        {
            // Two touches for zooming and rotating
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the difference in touch positions to rotate the camera
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                // Zooming: Calculate distance between the two touches and apply zoom
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                if (initialTouchDistance == 0)
                {
                    initialTouchDistance = currentDistance;
                }
                float zoomDelta = currentDistance - initialTouchDistance;
                Camera.main.fieldOfView -= zoomDelta * zoomSpeed;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 60f); // Clamp zoom levels

                // Rotation: Calculate angle between the two touches and apply rotation
                float angle = Vector2.SignedAngle(touch1PrevPos - touch2PrevPos, touch1.position - touch2.position);
                transform.Rotate(Vector3.up, -angle * rotationSpeed, Space.World);

                // Reset the initial distance for the next frame
                initialTouchDistance = currentDistance;
            }
        }
    }
}