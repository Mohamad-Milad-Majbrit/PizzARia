using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaceOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;            // Prefab to spawn
    public ARPlaneManager planeManager;         // ARPlaneManager reference
    public ARRaycastManager raycastManager;     // ARRaycastManager reference

    private bool hasPlacedObject = false;       // Ensures object is placed only once
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);

    }

    void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    private void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        // Only place once and only when new planes are detected
        if (hasPlacedObject || args.added.Count == 0)
            return;

        // Use the first newly detected plane
        ARPlane plane = args.added[0];

        // Raycast from the center of the screen to the plane
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            Instantiate(objectToPlace, pose.position, pose.rotation);
            hasPlacedObject = true;
        }
    }
}
