//Created by Julia Podlipensky
//Instantiate the Instruction Prefabs on the according Images with predefined offsets
//only instatiate one activePrefab


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class ImageHandler : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager m_TrackedImageManager;

    [SerializeField]
    private List<ReferencePrefabMapping> referencePrefabMappings;

    [SerializeField]
    private TMP_Text headerText;

    private Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();
    private List<string> imageOrder = new List<string>();
    private int nextImageIndex = 0;

    [Serializable]
    public struct ReferencePrefabMapping
    {
        public string imageName;
        public GameObject prefab;
        public Vector3 offset;
    }

    private void Awake()
    {
        // Initialize the image order list
        foreach (var mapping in referencePrefabMappings)
        {
            imageOrder.Add(mapping.imageName);
        }
    }

    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Debug.Log(eventArgs);
        
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.Log($"eventArgs.added for refImage: {trackedImage.referenceImage.name }");
            HandleTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            // Debug.Log($"eventArgs.updated for refImage: {trackedImage.referenceImage.name} ");
            UpdatePrefabTransform(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log($"eventArgs.removed for refImage: {trackedImage.referenceImage.name}");
            HandleRemovedImage(trackedImage);
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        // Ignore images that are not actively tracking
        if (trackedImage.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            return;

        // Check if this image already has an instantiated prefab
        if (instantiatedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            return;

        // Find the matching prefab mapping
        var mapping = referencePrefabMappings.Find(m => m.imageName == trackedImage.referenceImage.name);
        if (mapping.prefab != null)
        {
            // Calculate position with offset
            Vector3 positionWithOffset = trackedImage.transform.position + mapping.offset;

            // Instantiate the prefab
            GameObject newPrefab = Instantiate(mapping.prefab, positionWithOffset, trackedImage.transform.rotation);

            // Store the instantiated prefab
            instantiatedPrefabs[trackedImage.referenceImage.name] = newPrefab;

            // Update header text
            UpdateHeaderText(trackedImage);
        }
    }

    private void UpdatePrefabTransform(ARTrackedImage trackedImage)
    {
        // If we have an instantiated prefab for this image, update its transform
        if (instantiatedPrefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
        {
            // Find the matching mapping to get the offset
            var mapping = referencePrefabMappings.Find(m => m.imageName == trackedImage.referenceImage.name);

            prefab.transform.position = trackedImage.transform.position + mapping.offset;
            prefab.transform.rotation = trackedImage.transform.rotation;

            // Ensure prefab is visible when tracking
            prefab.SetActive(trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking);
        }
    }

    private void HandleRemovedImage(ARTrackedImage trackedImage)
    {
        if (instantiatedPrefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
        {
            prefab.SetActive(false);
        }
    }

    private void UpdateHeaderText(ARTrackedImage trackedImage)
    {
        nextImageIndex++;

        if (nextImageIndex < imageOrder.Count)
        {
            string nextImageName = imageOrder[nextImageIndex];
            headerText.text = $"You found the {trackedImage.referenceImage.name}, follow the instructions<br><i><size=70%>after that find the {nextImageName}</i></size>";
        }
        else
        {
            headerText.text = "All steps completed! Well done! <br>Now lets play a game";
        }
    }
}