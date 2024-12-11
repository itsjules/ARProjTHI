using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackImageInfo : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    
    private void Update() {
        if(Input.GetKey(KeyCode.Space)){
            ListAllImages();
        }
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            
        }
    }

    void ListAllImages()
    {
        Debug.Log(
            $"There are {m_TrackedImageManager.trackables.count} images being tracked.");


        foreach (var trackedImage in m_TrackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                    $"{trackedImage.transform.position}");
        }
    }

}