using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CanvasEventCameraAssigner : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            // Assign the main camera or AR camera as the Event Camera
            canvas.worldCamera = Camera.main;
        }
    }
}