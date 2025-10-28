using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RA : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager arTIM;
    [SerializeField] private GameObject[] arOModels2PLace;

    private Dictionary<string, GameObject> arModels = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> modelState = new Dictionary<string, bool>();


    void Start()
    {
        foreach (GameObject arModel in arOModels2PLace)
        {
            GameObject newARModel = Instantiate(arModel, Vector3.zero, Quaternion.identity);
            newARModel.name = arModel.name;

            arModels.Add(newARModel.name, newARModel);
            newARModel.SetActive(false);
            modelState.Add(newARModel.name, false);
        }
    }


    private void OnEnable()
    {
        arTIM.trackedImagesChanged += ImageFound;
    }


    private void OnDisable()
    {
        arTIM.trackedImagesChanged -= ImageFound;
    }


    private void ImageFound(ARTrackedImagesChangedEventArgs eventData)
    {
        foreach (var trackedImage in eventData.added)
        {
            showARModel(trackedImage);
        }

        foreach (var trackedImage in eventData.updated)
        {
            if(trackedImage.trackingState == TrackingState.Tracking)
            {
                showARModel(trackedImage);
            }
            else if (trackedImage.trackingState == TrackingState.Limited)
            {
                hideARModel(trackedImage);
            }
        }
    }


    private void showARModel(ARTrackedImage trackedImage)
    {
        bool isModelActive = modelState[trackedImage.referenceImage.name];

        if(!isModelActive)
        {
            GameObject arModel = arModels[trackedImage.referenceImage.name];
            arModel.transform.position = trackedImage.transform.position;

            arModel.SetActive(true);
            modelState[trackedImage.referenceImage.name] = true;
        }
        else
        {
            GameObject arModel = arModels[trackedImage.referenceImage.name];
            arModel.transform.position = trackedImage.transform.position;
        }    
    }


    private void hideARModel(ARTrackedImage trackedImage)
    {
        bool isModelActive = modelState[trackedImage.referenceImage.name];

        if(isModelActive)
        {
            GameObject arModel = arModels[trackedImage.referenceImage.name];
            arModel.SetActive(false);
            modelState[trackedImage.referenceImage.name] = false;
        }
    }


}
