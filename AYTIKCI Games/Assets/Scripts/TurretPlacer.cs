using UnityEngine;
using Cinemachine;

public class TurretPlacer : MonoBehaviour
{
    public Camera mainCamera;

    //public CinemachineFreeLook mainCamera;
    public TurretSelectionUI turretSelectionUI;
    private GameObject turretPreview; // Preview of the turret being placed
    private GameObject selectedTurret; // The currently selected turret
    private GameObject selectedPreviewTurret;
    void Update()
    {
        // Detect if a turret is selected
        selectedTurret = turretSelectionUI.GetSelectedTurret();
        selectedPreviewTurret = turretSelectionUI.GetSelectedPreviewTurret();
        if (selectedTurret == null)
            return; // No turret selected, do nothing
        
        // If the player starts a touch (on Android)
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Create a preview turret when the touch starts
                StartPlacingTurret(touch.position);
            }

            //if (touch.phase == TouchPhase.Moved)
            //{
            //    // Update the position of the preview turret as the touch moves
            //    UpdateTurretPosition(touch.position);
            //}

            if (touch.phase == TouchPhase.Ended)
            {
                // Place the turret when the touch ends
                PlaceTurret(touch.position);
            }
        }
    }

    void StartPlacingTurret(Vector2 touchPosition)
    {
        if (turretPreview != null)
            Destroy(turretPreview);

        // Instantiate a preview turret at the touch position with 50% opacity
        Vector3 worldPosition = GetTouchWorldPosition(touchPosition);
        turretPreview = Instantiate(selectedPreviewTurret, worldPosition, Quaternion.identity);
        
        // Change the opacity of the preview to 50%
        if (turretPreview.GetComponent<Renderer>() != null)
        {
            Color color = turretPreview.GetComponent<Renderer>().material.color;
            color.a = 0.5f; // Set opacity to 50%
            turretPreview.GetComponent<Renderer>().material.color = color;
        }
    }

    void UpdateTurretPosition(Vector2 touchPosition)
    {
        if (turretPreview == null)
            return;

        // Update preview position as touch moves
        Vector3 worldPosition = GetTouchWorldPosition(touchPosition);
        turretPreview.transform.position = worldPosition;
    }

    void PlaceTurret(Vector2 touchPosition)
    {
        if (turretPreview == null)
            return;

        // Get the cost of the selected turret (from its Turret script)
        int turretCost = selectedTurret.GetComponent<Turret>().cost;

        // Check if the player has enough gold to place the turret
        if (GoldManager.Instance.SpendGold(turretCost))
        {
            // If the player has enough gold, place the turret
            Vector3 worldPosition = GetTouchWorldPosition(touchPosition);
            Instantiate(selectedTurret, worldPosition, Quaternion.identity);

            // Remove the preview turret
            Destroy(turretPreview);
            selectedTurret = null;
            selectedPreviewTurret = null;
        }
        else
        {
            // If not enough gold, log a message
            Debug.Log("Not enough gold to place turret.");
            selectedTurret = null;
            selectedPreviewTurret = null;
            Destroy(turretPreview);
        }
    }

    // Converts touch position to world position
    Vector3 GetTouchWorldPosition(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point; // Return where the ray hit
        }
        return ray.GetPoint(10f); // Default position if nothing is hit
    }
}