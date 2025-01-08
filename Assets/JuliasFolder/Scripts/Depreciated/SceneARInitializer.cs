using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SceneARInitializer : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private XRReferenceImageLibrary referenceImageLibrary; // Assign this via Inspector

    private void Awake()
    {
        if (trackedImageManager == null)
        {
            trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        }

        if (trackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager is missing from the scene!");
            return;
        }

        if (referenceImageLibrary == null)
        {
            Debug.LogError("ReferenceImageLibrary is not assigned!");
            return;
        }

        InitializeImageTracking();
    }

    private void InitializeImageTracking()
    {
        Debug.Log("Initializing Image Tracking...");

        // Ensure ARTrackedImageManager is properly set up
        trackedImageManager.enabled = false; // Stop tracking temporarily
        trackedImageManager.referenceLibrary = referenceImageLibrary; // Assign the library
        trackedImageManager.enabled = true;  // Restart tracking with the new library

        Debug.Log($"Image Tracking Initialized with Library: {referenceImageLibrary.name}");
    }
}