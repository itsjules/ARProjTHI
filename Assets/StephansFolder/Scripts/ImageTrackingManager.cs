using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class ImageTrackingManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        Debug.Log(" triggered");
        
        foreach (var trackedImage in args.added)
        {
            
            UnityEngine.Debug.Log($"Bild erkannt: {trackedImage.referenceImage.name}");

            if (trackedImage.referenceImage.name == "start_reference") // Der Name des Bildes aus der XR Reference Image Library
            {
                UnityEngine.Debug.Log("Bild erfolgreich getrackt! Wechsel zur Szene Introduction.");
                SceneManager.LoadScene("Introduction"); // Szene wechseln
            }
        }
    }
}
