using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageHandler : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    [SerializeField]
    private List<ReferencePrefabMapping> referencePrefabMappings;

    private Dictionary<string, ReferencePrefabMapping> prefabDictionary = new Dictionary<string, ReferencePrefabMapping>();

    private GameObject activePrefab = null; // Only one active prefab at a time
    private ARTrackedImage currentTrackedImage = null; // Tracks the currently displayed image

    [Serializable]
    public struct ReferencePrefabMapping
    {
        public string imageName; // exact name of the refimage
        public GameObject prefab; // Prefab associated with the image
        public Vector3 offset; // Offset to apply to the prefab
    }

    public static ImageHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        // Initialize the prefab dictionary
        foreach (var mapping in referencePrefabMappings)
        {
            if (!prefabDictionary.ContainsKey(mapping.imageName))
            {
                prefabDictionary[mapping.imageName] = mapping;
            }
        }
    }

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;
    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Handle newly detected images
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            HandleImageChange(trackedImage);
        }

        // Handle updates to tracked images (e.g., position updates)
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            HandleImageChange(trackedImage);
        }

        // Optional cleanup for images no longer being tracked
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (currentTrackedImage == trackedImage)
            {
                DestroyActivePrefab();
                currentTrackedImage = null;
            }
        }
    }

    private void HandleImageChange(ARTrackedImage trackedImage)
    {
        // Ignore if the image is already being tracked and hasn't changed
        if (currentTrackedImage == trackedImage)
        {
            if (activePrefab != null)
            {
                // Update the prefab's position and rotation if it is already active
                UpdatePrefabTransform(trackedImage);
            }
            return;
        }

        // Switch to the new image
        DestroyActivePrefab(); // Clear the existing prefab and image
        currentTrackedImage = trackedImage;

        // Instantiate prefab for the new image
        if (prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
        {
            Vector3 positionWithOffset = trackedImage.transform.position + mapping.offset;
            activePrefab = Instantiate(mapping.prefab, positionWithOffset, trackedImage.transform.rotation);
            Debug.Log($"Prefab {activePrefab.name} instantiated for image: {trackedImage.referenceImage.name}");
        }
    }

    private void UpdatePrefabTransform(ARTrackedImage trackedImage)
    {
        if (activePrefab != null)
        {
            activePrefab.transform.position = trackedImage.transform.position + prefabDictionary[trackedImage.referenceImage.name].offset;
            activePrefab.transform.rotation = trackedImage.transform.rotation;
        }
    }

    private void DestroyActivePrefab()
    {
        if (activePrefab != null)
        {
            Destroy(activePrefab);
            activePrefab = null;
        }
    }
}
