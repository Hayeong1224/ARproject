using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedMultiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trakedPrefabs;

    private Dictionary<string, GameObject> spawnedObjects=new Dictionary<string, GameObject>();
    private ARTrackedImageManager imageManager;

    private void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        foreach(GameObject obj in trakedPrefabs)
        {
            GameObject clone = Instantiate(obj);
            clone.name= obj.name;
            clone.SetActive(false);
            spawnedObjects.Add(clone.name, clone);
        }
    }
    private void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    private void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var trackedImage in args.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (var trackedImage in args.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (var trackedImage in args.removed)
        {
            spawnedObjects[trackedImage.name].SetActive(false);
        }
    }
    private void UpdateImage(ARTrackedImage Image)
    {
        string name = Image.referenceImage.name;
        GameObject obj = spawnedObjects[name];
       

        if (Image.trackingState== TrackingState.Tracking)
        {
            obj.transform.position = Image.transform.position;
            obj.transform.rotation = Image.transform.rotation;
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }
 
}
