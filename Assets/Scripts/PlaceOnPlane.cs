using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlaceOnPlane : MonoBehaviour
{
    public ARPlaneManager planeManager;         // ARPlaneManager reference
    public ARRaycastManager raycastManager;     // ARRaycastManager reference
    public GameObject hintScanUI;      
    public GameObject hintTapUI;

    public LayerMask pizzaLayerMask;

    private bool planesDetected = false;
    private bool placed = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public UnityEvent OnPizzaPlaced;

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

        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;
                float plateSize = OrderManager.Instance.GetFloatSize()*2;

                GameObject newPizza = Instantiate(OrderManager.Instance.currentPizza.arBaseModelPrefab, pose.position, pose.rotation);
                newPizza.transform.localScale = new Vector3(plateSize, plateSize, plateSize);

                PizzaController toppingController = newPizza.GetComponent<PizzaController>();
                if (toppingController == null)
                {
                    toppingController = newPizza.AddComponent<PizzaController>();
                }
                toppingController.Initialize(plateSize, pizzaLayerMask);


                int ingredientAmount = OrderManager.Instance.GetFloatIngredientAmount();
                List<IngredientData> activeIngredients = new List<IngredientData>();
                foreach (var ingredient in OrderManager.Instance.allIngredients)
                {
                    if (OrderManager.Instance.IsIgredientSelected(ingredient))
                    {
                        toppingController.SpawnInitialBatch(ingredient, ingredientAmount);
                    }
                }
                toppingController.StartPop(new Vector3(plateSize, plateSize, plateSize));
                placed = true;
                OnPizzaPlaced?.Invoke();

                hintTapUI?.SetActive(false);

                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
                planeManager.enabled = false; 
            }
        }
    }



    /*
    IEnumerator SpawnIngredients(List<IngredientData> ingredients, GameObject pizzaObject, int totalCount, float parentScale, PizzaToppingController controller)
    {
        Transform pizzaTransform = pizzaObject.transform;


        float normalizedRadius = 0.35f;


        for (int i = 0; i < totalCount; i++)
        {
            IngredientData ingredient = ingredients[i];

            float r = normalizedRadius * Mathf.Sqrt((float)i / totalCount);
            float theta = i * Mathf.PI * (3f - Mathf.Sqrt(5f));

            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);

            Vector3 rayStartLocal = new Vector3(x, 0.2f, z);
            Vector3 rayStartWorld = pizzaTransform.TransformPoint(rayStartLocal);

            float spawnDelay = 0.05f;

            if (Physics.Raycast(rayStartWorld, -pizzaTransform.up, out RaycastHit hitInfo, 0.5f, pizzaLayerMask))
            {
     
                GameObject newIngredient = Instantiate(ingredient.arModelPrefab, hitInfo.point, Quaternion.identity);
                Vector3 originalPrefabScale = ingredient.arModelPrefab.transform.localScale;
                newIngredient.transform.SetParent(pizzaTransform, true);
                newIngredient.transform.localScale = new Vector3(
                    originalPrefabScale.x / parentScale,
                    originalPrefabScale.y / parentScale,
                    originalPrefabScale.z / parentScale
                );

                newIngredient.transform.up = hitInfo.normal;

                newIngredient.transform.Rotate(0, Random.Range(0, 360), 0, Space.Self);

                controller.RegisterTopping(ingredient, newIngredient);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }*/
}
