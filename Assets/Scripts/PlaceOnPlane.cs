using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

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
                float plateSize = OrderManager.Instance.GetFloatSize()*2;

                GameObject newPizza = Instantiate(OrderManager.Instance.currentPizza.arBaseModelPrefab, pose.position, pose.rotation);
                newPizza.transform.localScale = new Vector3(plateSize, plateSize, plateSize);


                int ingredientAmount = OrderManager.Instance.GetFloatIngredientAmount();
                int totalIngredients = OrderManager.Instance.allIngredients.Count;
                //float pizzaRadius = OrderManager.Instance.GetFloatSize() -0.06f;

                // All ingredients with their frequency of occurrence
                List<IngredientData> ingredients = new List<IngredientData> { };
                for (int i = 0; i < totalIngredients; i++)
                {
                    IngredientData ingredient = OrderManager.Instance.allIngredients[i];
                    for(int j = 0; j < ingredientAmount; j++)
                    {
                        ingredients.Add(ingredient);
                    }
                }
                int totalIngredientsAmount = ingredients.Count;


                // Mix the list so that one type of ingredient is not always inside and others outside.
                for (int i = 0; i < totalIngredientsAmount; i++)
                {
                    IngredientData temp = ingredients[i];
                    int randomIndex = Random.Range(i, totalIngredientsAmount);
                    ingredients[i] = ingredients[randomIndex];
                    ingredients[randomIndex] = temp;
                }


                // Spread the ingredients on the pizza
                StartCoroutine(SpawnIngredients(ingredients, newPizza, totalIngredientsAmount, plateSize));
                /*for (int i = 0; i < totalIngredientsAmount; i++)
                {
                    IngredientData ingredient = ingredients[i];

                    float r = pizzaRadius * Mathf.Sqrt((float)i / totalIngredientsAmount);

                    float theta = i * Mathf.PI * (3f - Mathf.Sqrt(5f));

                    float x = r * Mathf.Cos(theta);
                    float z = r * Mathf.Sin(theta);
                    float y = Random.Range(0.1f, 0.13f);

                    Vector3 localPos = new Vector3(x, y, z); 
                    Vector3 finalPos = pose.position + (pose.rotation * localPos);

                    Quaternion randomSpin = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    Quaternion finalRotation = pose.rotation * randomSpin;

                    GameObject newIngredient = Instantiate(ingredient.arModelPrefab, finalPos, finalRotation);

                    Rigidbody rb = newIngredient.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    }
                }*/


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




    IEnumerator SpawnIngredients(List<IngredientData> ingredients, GameObject pizzaObject, int totalCount, float parentScale)
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
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
