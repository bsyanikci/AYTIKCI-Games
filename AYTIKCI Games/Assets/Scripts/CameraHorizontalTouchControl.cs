using UnityEngine;
using Cinemachine;
using System.Collections;


public class CameraHorizontalTouchControl : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera; // No need to expose this in the Inspector
    public Transform target;  // The target object the camera is rotating around
    public float rotationSpeed = 1.0f;  // Speed of the camera rotation
    private Vector2 lastTouchPosition;  // To store the position of the touch
    private float currentRotation = 0f;  // To track the current rotation of the camera
    public float distance = 25f;

    public float transitionDuration = 2f; // Duration of the smooth transition
    public float targetY = 11f; // Target Y position in world space
    private float initialY; // Initial Y position in world space

    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        // Ensure we have a valid CinemachineFreeLook component attached
        if (freeLookCamera != null)
        {
            // Store the initial Y position of the FreeLook Camera
            initialY = freeLookCamera.transform.position.y;

            // Start the smooth transition to the target Y position
            StartCoroutine(SmoothCameraYPositionTransition(initialY, targetY, transitionDuration));
        }
        else
        {
            Debug.LogError("CinemachineFreeLook component not found.");
        }
    }

    // Coroutine to smoothly move the camera's Y position
    IEnumerator SmoothCameraYPositionTransition(float startY, float targetY, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new Y position using Mathf.Lerp
            float currentY = Mathf.Lerp(startY, targetY, elapsedTime / duration);

            // Update the camera's position, keeping the X and Z positions unchanged
            Vector3 newPosition = freeLookCamera.transform.position;
            newPosition.y = currentY;
            freeLookCamera.transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera ends up at the target Y position at the end
        Vector3 finalPosition = freeLookCamera.transform.position;
        finalPosition.y = targetY;
        freeLookCamera.transform.position = finalPosition;
    }


    // Map world space Y position to Cinemachine FreeLook m_YAxis.Value range (0 to 1)
    private float MapWorldYToAxisValue(float worldY)
    {
        // This assumes the camera's Y position range (adjust based on your setup)
        // You need to manually map the world Y to the FreeLook's normalized Y range
        float cameraHeight = freeLookCamera.m_Lens.OrthographicSize; // Get camera's height (adjust if not orthographic)

        // If not using orthographic mode, you might need to consider world space to view space mapping
        float normalizedValue = Mathf.InverseLerp(initialY, targetY, worldY);

        return Mathf.Clamp01(normalizedValue); // Ensure it's between 0 and 1
    }

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
                Vector3 direction = new Vector3(Mathf.Sin(currentRotation), 0.75f, Mathf.Cos(currentRotation)) * distance;  // Distance of 10 units
                transform.position = target.position + direction;  // Update camera position
                transform.LookAt(target);  // Always look at the target

                // Update the last touch position
                lastTouchPosition = touch.position;
            }
        }
    }
}
