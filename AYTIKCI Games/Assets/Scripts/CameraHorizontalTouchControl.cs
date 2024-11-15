using UnityEngine;
using Cinemachine;


public class CameraHorizontalTouchControl : MonoBehaviour
{
    public Transform target;  // The target object the camera is rotating around
    public float rotationSpeed = 1.0f;  // Speed of the camera rotation
    private Vector2 lastTouchPosition;  // To store the position of the touch
    private float currentRotation = 0f;  // To track the current rotation of the camera
    public float distance = 25f;

    void Update()
    {
        // Only respond to touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);  // Get the first touch

            // Start tracking the touch movement
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }

            // When the touch moves, rotate the camera horizontally
            if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the horizontal movement
                float deltaX = touch.position.x - lastTouchPosition.x;

                // Adjust the camera rotation based on the horizontal drag
                currentRotation += deltaX * rotationSpeed * Time.deltaTime;

                // Rotate the camera around the target (y-axis)
                Vector3 direction = new Vector3(Mathf.Sin(currentRotation), 0.5f, Mathf.Cos(currentRotation)) * distance;  // Distance of 10 units
                transform.position = target.position + direction;  // Update camera position
                transform.LookAt(target);  // Always look at the target

                // Update the last touch position
                lastTouchPosition = touch.position;
            }
        }
    }
}
