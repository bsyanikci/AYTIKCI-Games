using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;  // Reference to the Cinemachine FreeLook Camera
    public float rotateSpeed = 0.1f;  // Speed of the rotation
    public float zoomSpeed = 1f;  // Speed of zooming in/out (optional)

    private Vector2 touchDelta;  // Touch movement delta

    void Update()
    {
        // Check if there are touch inputs
        if (Input.touchCount > 0)
        {
            // Handle single touch rotation
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Calculate touch delta movement
                touchDelta = touch.deltaPosition;

                // Rotate the camera based on touch movement
                RotateCamera(touchDelta);
            }

            // Optional: Zoom in/out based on touch pinch
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Calculate the distance between the two touches
                float prevDistance = (touch1.position - touch2.position).magnitude;
                float currentDistance = (touch1.position - touch2.position).magnitude;

                // Zoom the camera based on the pinch gesture
                ZoomCamera(currentDistance - prevDistance);
            }
        }
    }

    // Rotate the camera based on touch movement delta
    private void RotateCamera(Vector2 delta)
    {
        // Adjust the camera's horizontal and vertical rotation based on touch delta
        freeLookCamera.m_XAxis.Value += delta.x * rotateSpeed;  // Rotate horizontally
        //freeLookCamera.m_YAxis.Value -= delta.y * rotateSpeed;  // Rotate vertically (if needed)
    }

    // Zoom in/out based on pinch gesture
    private void ZoomCamera(float delta)
    {
        // Adjust the Field of View to simulate zooming in/out
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView - delta * zoomSpeed, 20f, 80f);
    }
}