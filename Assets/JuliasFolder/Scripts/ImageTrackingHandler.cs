using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System;

public class ImageTrackingHandler : MonoBehaviour
{
    public static ImageTrackingHandler Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    private Dictionary<string, GameObject> instantiatedPrefabs = new Dictionary<string, GameObject>();

    [SerializeField]
    private List<ReferencePrefabMapping> referencePrefabMappings;

    [Serializable]
    public struct ReferencePrefabMapping
    {
        public string imageName;
        public GameObject prefab;
        public Vector3 offset;
    }

    private bool isCurrentStepImageProcessed = false;

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
        
        foreach (var trackedImage in args.added)
        {
            HandleTrackedImage(trackedImage);
        }
        foreach (var trackedImage in args.updated)
        {
            UpdatePrefabTransform(trackedImage);
            ProcessStepImage(trackedImage);
        }
        foreach (var trackedImage in args.removed)
        {
            HandleRemovedImage(trackedImage);
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        // // Ignore images that are not actively tracking (led to bugs bcs sometimes there are issues with tracking state of images on mobile build)
        // if (trackedImage.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking){
        //     return;
        // }

        // Check if this image already has an instantiated prefab
        if (instantiatedPrefabs.ContainsKey(trackedImage.referenceImage.name)){
            return;
        }
        
        // Find the matching prefab mapping
        var mapping = referencePrefabMappings.Find(m => m.imageName == trackedImage.referenceImage.name);


        if (mapping.prefab != null)
        {
            //Debug.Log(mapping.prefab);
            
            Vector3 positionWithOffset = trackedImage.transform.position + mapping.offset;

            GameObject newPrefab = Instantiate(mapping.prefab, positionWithOffset, trackedImage.transform.rotation);

            // Store the instantiated prefab
            instantiatedPrefabs[trackedImage.referenceImage.name] = newPrefab;

            //Deactivate Prefab if this is not the required image for the current step 
            var currentStep = StepManager.Instance.GetCurrentStep();
            if (currentStep == null || currentStep.trackingImageName != trackedImage.referenceImage.name)
            {
                newPrefab.SetActive(false);
            }
        }
    }

    private void UpdatePrefabTransform(ARTrackedImage trackedImage)
    {
        //Skip if this is not the required image for the current step
        var currentStep = StepManager.Instance.GetCurrentStep();
        if (currentStep == null || currentStep.trackingImageName != trackedImage.referenceImage.name)
        {
            return;
        }
        
        // If we have an instantiated prefab for this image, update its transform
        if (instantiatedPrefabs.TryGetValue(trackedImage.referenceImage.name, out GameObject prefab))
        {
            // Find the matching mapping to get the offset
            var mapping = referencePrefabMappings.Find(m => m.imageName == trackedImage.referenceImage.name);

            prefab.transform.position = trackedImage.transform.position + mapping.offset;
            prefab.transform.rotation = Quaternion.Slerp(prefab.transform.rotation, trackedImage.transform.rotation, Time.deltaTime * 0.5f);

            
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

    private void ProcessStepImage(ARTrackedImage trackedImage){

        var currentStep = StepManager.Instance.GetCurrentStep(); 
        if (currentStep != null &&
            currentStep.trackingImageName == trackedImage.referenceImage.name &&
            !isCurrentStepImageProcessed)
        {
            isCurrentStepImageProcessed = true;

            StepManager.Instance.OnImageTracked(trackedImage.referenceImage.name);
        }
    }

    public void ResetProcessingState()
    {
        isCurrentStepImageProcessed = false;
    }
}