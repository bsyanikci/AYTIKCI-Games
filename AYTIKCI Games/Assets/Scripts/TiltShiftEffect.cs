using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TiltShiftEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;

    // Parameters for tilt-shift effect
    public float focusDistance = 10f;  // Where the focus should be
    public float aperture = 1f;        // Depth of field strength
    public float blurRadius = 2f;      // Blur radius for tilt-shift effect

    void Start()
    {
        // Get the Post-process Profile attached to the volume
        if (postProcessVolume.profile.TryGetSettings(out depthOfField))
        {
            // Initialize depth of field settings
            SetTiltShiftEffect();
        }
        else
        {
            Debug.LogError("No Depth of Field effect found in Post-process Profile.");
        }
    }

    void SetTiltShiftEffect()
    {
        // Adjust the depth of field effect to create a basic tilt-shift effect
        depthOfField.focusDistance.value = focusDistance;
        depthOfField.aperture.value = aperture;

        // Optionally, you can also set other parameters here for additional effects
    }

    void Update()
    {
        // Optionally, you can change focus or aperture dynamically during runtime
        if (depthOfField != null)
        {
            depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, focusDistance, Time.deltaTime * 2f);
            depthOfField.aperture.value = Mathf.Lerp(depthOfField.aperture.value, aperture, Time.deltaTime * 2f);
        }
    }
}