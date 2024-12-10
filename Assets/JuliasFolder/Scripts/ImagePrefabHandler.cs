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

    private Scene currentScene; //scene this component is working in

    [SerializeField]
    private Text debugText;//debug Text 


    private void Awake()
    {
        
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        currentTrackedPrefab=null;
    }

    


    private void OnEnable()
    {
        if (trackedImageManager != null)
        {
            currentTrackedPrefab=null;
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            currentScene=SceneManager.GetActiveScene();
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
            // Destroy(currentTrackedPrefab);
            // currentTrackedPrefab=null;
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Handle added images
        foreach (var trackedImage in args.added)
        {
            if (trackedImage.referenceImage.name=="PrintKiosk" && currentScene.name=="PrintStep"){
                HandleTrackedImage(trackedImage);
            }
            if (trackedImage.referenceImage.name=="ValidationKiosk" && currentScene.name=="ValidationStep"){
                HandleTrackedImage(trackedImage);
            }
            if (trackedImage.referenceImage.name=="MoneyKiosk" && currentScene.name=="ChargingStep"){
                HandleTrackedImage(trackedImage);
            }
             if (trackedImage.referenceImage.name=="finalQR" && currentScene.name=="FinalQRStep"){
                HandleTrackedImage(trackedImage);
            }
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

            debugText.text=$"currentPrefab is: {currentTrackedPrefab}";

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
            //currentTrackedPrefab.SetActive(trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
            currentTrackedPrefab.SetActive(true);
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
