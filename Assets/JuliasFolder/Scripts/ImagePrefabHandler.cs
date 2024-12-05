using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARCore;

public class ImagePrefabHandler : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private GameObject instructionPrefab; // Prefab to instantiate

    [SerializeField]
    private Vector3 prefabOffset; // Offset for positioning the prefab

    private GameObject currentTrackedPrefab;

    [SerializeField]
    private Button continueBttn; // Button to activate
    [SerializeField]
    private Canvas hintCanvas; // Button to activate



    private void Awake()
    {
        
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }


    private void OnEnable()
    {
        if (trackedImageManager != null)
        {
            currentTrackedPrefab=null;
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        else
        {
            Debug.LogError("ARTrackedImageManager is missing! Please attach it to the GameObject.");
        }
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Handle added images
        foreach (var trackedImage in args.added)
        {
            HandleTrackedImage(trackedImage);
        }

        // Handle updated images
        foreach (var trackedImage in args.updated)
        {
            UpdateTrackedImage(trackedImage);
        }

        // Handle removed images
        foreach (var trackedImage in args.removed)
        {
            RemoveTrackedImage();
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        Debug.Log($"Image detected: {trackedImage.referenceImage.name}");

        if (instructionPrefab != null && currentTrackedPrefab == null)
        {
            // Instantiate prefab at tracked image position with offset
            Vector3 positionWithOffset = trackedImage.transform.position + prefabOffset;
            currentTrackedPrefab = Instantiate(instructionPrefab, positionWithOffset, trackedImage.transform.rotation);

            // Disable hint Canvas if assigned
            if (hintCanvas!=null)
            {
                hintCanvas.gameObject.SetActive(false);
            }
            
            // Enable continue button if assigned
            if (continueBttn != null)
            {
                continueBttn.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Prefab is already instantiated or not assigned.");
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if (currentTrackedPrefab != null)
        {
            // Update prefab's position and rotation
            currentTrackedPrefab.transform.position = trackedImage.transform.position + prefabOffset;
            currentTrackedPrefab.transform.rotation = trackedImage.transform.rotation;

            // Ensure the prefab is active only when tracking
            currentTrackedPrefab.SetActive(trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
        }
    }

    private void RemoveTrackedImage()
    {
        if (currentTrackedPrefab != null)
        {
            Destroy(currentTrackedPrefab);
            currentTrackedPrefab = null;

            // Disable continue button if assigned
            if (continueBttn != null)
            {
                continueBttn.gameObject.SetActive(false);
            }
        }
    }
}
