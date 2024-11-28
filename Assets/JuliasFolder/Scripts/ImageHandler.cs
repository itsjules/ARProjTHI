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

    private Dictionary<string, ReferencePrefabMapping> prefabDictionary = new Dictionary<string, ReferencePrefabMapping>();
    private List<string> imageOrder = new List<string>(); // Ordered list of image names (safely can make it work only over referencePrefabMappings)
    private int nextImageIndex = 0;

    private GameObject activePrefab = null; // The current active prefab
    private ARTrackedImage currentTrackedImage = null; // The currently tracked image

    // [SerializeField]
    // private GameObject debugPosMarking;

    [SerializeField]
    private TMP_Text headerText;

    [Serializable]
    public struct ReferencePrefabMapping
    {
        public string imageName; 
        public GameObject prefab; 
        public Vector3 offset; 
    }

    private void Awake()
    {
        // Initialize the prefab dictionary
        foreach (var mapping in referencePrefabMappings)
        {
            if (!prefabDictionary.ContainsKey(mapping.imageName))
            {
                prefabDictionary[mapping.imageName] = mapping;
                imageOrder.Add(mapping.imageName);
            }
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
        // Handle newly added or updated tracked images (removed doesn't work and on my smartphone added part of the event also doesnt trigger, so fallback on doing everything in update)
        foreach (var trackedImage in eventArgs.added)
        {
            HandleTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            HandleTrackedImage(trackedImage);
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        // Ignore images that are not actively tracked
        if (trackedImage.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            return;

        // If the image is already the currently tracked one, just update the prefab's transform
        if (currentTrackedImage == trackedImage)
        {
            UpdatePrefabTransform(trackedImage);
            return;
        }

        //Debug.Log($"Switching prefab to match image: {trackedImage.referenceImage.name}");
        
        DestroyActivePrefab();

        currentTrackedImage = trackedImage;

        // Instantiate the prefab for the new image
        if (prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
        {
            Vector3 positionWithOffset = trackedImage.transform.position + mapping.offset;
            activePrefab = Instantiate(mapping.prefab, positionWithOffset, trackedImage.transform.rotation);
            // Debug.Log($"Prefab {activePrefab.name} instantiated for image: {trackedImage.referenceImage.name}");

            //visual debug
            // Instantiate(debugPosMarking,positionWithOffset, trackedImage.transform.rotation);

            //update headerText on Canvas (will shift this to StepManager later)
            nextImageIndex++;
            // UpdateHeaderText();

        }
    }

    private void UpdatePrefabTransform(ARTrackedImage trackedImage)
    {
        if (activePrefab != null && prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
        {
            activePrefab.transform.position = trackedImage.transform.position + mapping.offset;
            activePrefab.transform.rotation = trackedImage.transform.rotation;
        }
    }

    private void DestroyActivePrefab()
    {
        if (activePrefab != null)
        {
            // Debug.Log($"Destroying prefab: {activePrefab.name}");
            Destroy(activePrefab);
            activePrefab = null;
        }
    }

    private void UpdateHeaderText()
    {
        if (nextImageIndex < imageOrder.Count)
        {
            string nextImageName = imageOrder[nextImageIndex];
            headerText.text = $"You found the {currentTrackedImage.referenceImage.name}, follow the instructions<br><i><size=70%>after that find the {nextImageName}</i></size>";
        }
        else
        {
            headerText.text = "All steps completed! Well done! <br>Now lets play a game";
        }

        Debug.Log($"Change Header Text, repaint canvas");
    }
}
