using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageHandler : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    // A dictionary to map reference image names to prefabs and offsets.
    [SerializeField]
    private List<ReferencePrefabMapping> referencePrefabMappings;

    private Dictionary<string, ReferencePrefabMapping> prefabDictionary = new Dictionary<string, ReferencePrefabMapping>();

    //-------------------Old code before activePrefab--------------------------
    //private Dictionary<Guid, GameObject> instantiatedPrefabs = new Dictionary<Guid, GameObject>();
    private GameObject activePrefab = null; // Only one active prefab at a time

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
            // Debug.Log($"Mapping: {mapping.imageName} -> {mapping.prefab.name}");
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
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            //-------------------Old code before activePrefab--------------------------

            // string imageName = trackedImage.referenceImage.name;

            // // Check if the reference image has a corresponding prefab
            // if (prefabDictionary.TryGetValue(imageName, out ReferencePrefabMapping mapping) && 
            //     !instantiatedPrefabs.ContainsKey(trackedImage.referenceImage.guid))
            // {
            //     // Instantiate the prefab with an offset
            //     Vector3 positionWithOffset = trackedImage.transform.position + mapping.offset;

            //     // //test the offset problem
            //     // GameObject instantiatedPrefab = Instantiate(mapping.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
            //     // Debug.Log($"Tracked Image {imageName} Center Position: {trackedImage.transform.position}");

            //     //right one with the offset
            //     GameObject instantiatedPrefab = Instantiate(mapping.prefab, positionWithOffset, trackedImage.transform.rotation);

            //     instantiatedPrefabs[trackedImage.referenceImage.guid] = instantiatedPrefab;
            // }

            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                DestroyActivePrefab(); // Clear existing prefab
                // Debug.Log($"Added: {trackedImage.referenceImage.name}");

                if (prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
                {
                    Vector3 positionWithOffset = trackedImage.transform.position + mapping.offset;
                    activePrefab = Instantiate(mapping.prefab, positionWithOffset, trackedImage.transform.rotation);
                    Debug.Log($"Prefab {activePrefab.name} instantiated for image: {trackedImage.referenceImage.name}");
                }
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //-------------------Old code before activePrefab--------------------------
            // if (instantiatedPrefabs.TryGetValue(trackedImage.referenceImage.guid, out GameObject prefab))
            // {
            //     // Update position with the specific offset
            //     if (prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
            //     {
            //         prefab.transform.position = trackedImage.transform.position+ mapping.offset;
            //         prefab.transform.rotation = trackedImage.transform.rotation;
            //     }
            // }

            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking && 
            prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
        {
            if (activePrefab != null)
            {
                activePrefab.transform.position = trackedImage.transform.position + mapping.offset;
                activePrefab.transform.rotation = trackedImage.transform.rotation;
            }
            Debug.Log($"Updated Image: {trackedImage.referenceImage.name}, Tracking State: {trackedImage.trackingState}");
        }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //-------------------Old code before activePrefab--------------------------
            // if (instantiatedPrefabs.TryGetValue(trackedImage.referenceImage.guid, out GameObject prefab))
            // {
            //     Destroy(prefab);
            //     instantiatedPrefabs.Remove(trackedImage.referenceImage.guid);
            // }

            // if (activePrefab != null && prefabDictionary.TryGetValue(trackedImage.referenceImage.name, out ReferencePrefabMapping mapping))
            // {
            //     // Destroy the active prefab if it corresponds to the removed image
            //     DestroyActivePrefab();


            // }
            Debug.Log($"Removed: {trackedImage.referenceImage.name}");
            DestroyActivePrefab();


        }

        // // cleanup if no tracked images are left
        // if (m_TrackedImageManager.trackables.count == 0)
        // {
        //     DestroyActivePrefab();
        // }


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
