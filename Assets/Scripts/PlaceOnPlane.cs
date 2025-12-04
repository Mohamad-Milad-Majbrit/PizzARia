using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlaceOnPlane : MonoBehaviour
{
    public ARPlaneManager planeManager;         // ARPlaneManager reference
    public ARRaycastManager raycastManager;     // ARRaycastManager reference
    public GameObject hintScanUI;      
    public GameObject hintTapUI;       

    private bool planesDetected = false;
    private bool placed = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
    }
    void OnDisable()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    void Start()
    {
        planeManager.enabled = false;
        raycastManager.enabled = false;

        hintScanUI?.SetActive(false);
        hintTapUI?.SetActive(false);
    }


    public void StartARPlacement()
    {
        planeManager.enabled = true;
        raycastManager.enabled = true;

        hintScanUI?.SetActive(true);
    }

    private void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        if (planesDetected || args.added.Count == 0)
            return;

        planesDetected = true;

        hintScanUI?.SetActive(false);
        hintTapUI?.SetActive(true);
    }

    void Update()
    {
        Debug.Log("Update");
        if (!planesDetected || placed)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return; 
        }

        Debug.Log("keine Returns");
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("TouchPhase");
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Debug.Log("Raycst");
                Pose pose = hits[0].pose;

                Instantiate(OrderManager.Instance.currentPizza.arBaseModelPrefab, pose.position, pose.rotation);

                Vector3 ingredientPosition = pose.position;
                ingredientPosition.y += 0.1f;
                

                foreach (IngredientData ingredient in OrderManager.Instance.allIngredients)
                {
                    //ingredientPosition.x = Random.Range(-10, 10) / 100f;
                    //ingredientPosition.z = Random.Range(-10, 10) / 100f;

                    Instantiate(ingredient.arModelPrefab, ingredientPosition, pose.rotation);

                }


                placed = true;
                hintTapUI?.SetActive(false);

                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false; 
            }
        }
    }
}
