using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageHandler : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    public static ImageHandler Instance { get; private set; }

    private void Awake()
    {
        transform.parent = null;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }


    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            // foreach (tracka)
           
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            
        }
    }

    void Update(){
        //get just the prefab without Instantiation on Image
        Debug.Log(m_TrackedImageManager.trackedImagePrefab.transform.position);
        //get count of Instantiated Prefabs on Images from RefLib
        Debug.Log(m_TrackedImageManager.trackables.count);
    }




}
