using UnityEngine;
using UnityEngine.UI;

public class TurretSelectionUI : MonoBehaviour
{
    public Button[] turretButtons; // UI Buttons for selecting turrets
    public GameObject[] turretPrefabs; // Turret prefabs to spawn
    public GameObject[] turretPreviewPrefabs; // Turret prefabs to spawn
    public Button cancelButton; // Cancel button to reset turret selection
    private GameObject selectedTurret;
    private GameObject selectedPreviewTurret;

    void Start()
    {
        // Setup button listeners to select a turret prefab
        for (int i = 0; i < turretButtons.Length; i++)
        {
            int index = i; // Capture index in closure
            turretButtons[i].onClick.AddListener(() => SelectTurret(index));
        }

        // Setup listener for the cancel button
        cancelButton.onClick.AddListener(CancelSelection);
    }

    void SelectTurret(int index)
    {
        selectedTurret = turretPrefabs[index];
        selectedPreviewTurret = turretPreviewPrefabs[index];
        Debug.Log("Selected turret: " + selectedTurret.name);
    }

    public void CancelSelection()
    {
        selectedTurret = null;
        Debug.Log("Turret selection canceled.");
    }

    public GameObject GetSelectedTurret()
    {
        return selectedTurret;
    }
    public GameObject GetSelectedPreviewTurret()
    {
        return selectedPreviewTurret;
    }
    
}