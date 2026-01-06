using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class PlaceOnPlane : MonoBehaviour
{
    public ARPlaneManager planeManager;         // ARPlaneManager reference
    public ARRaycastManager raycastManager;     // ARRaycastManager reference
    public GameObject hintScanUI;      
    public GameObject hintTapUI;

    public LayerMask pizzaLayerMask;

    public SizeControllerPizza mainSizeController;
    public SizeControllerPizza compareSizeController;

    private bool planesDetected = false;
    private bool placed = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public UnityEvent OnPizzaPlaced;
    public UnityEvent OnPizzaCompare;

    private GameObject mainPizza;
    private GameObject comparePizza;
    private Vector3 mainPizzaOriginalPosition;


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

                mainPizza = SpawnPizza(pose.position, pose.rotation, mainSizeController);
                mainPizzaOriginalPosition = pose.position;
                /*mainPizza = Instantiate(OrderManager.Instance.currentPizza.arBaseModelPrefab, pose.position, pose.rotation);
                mainPizza.transform.localScale = new Vector3(plateSize, plateSize, plateSize);

                PizzaController toppingController = mainPizza.GetComponent<PizzaController>();
                if (toppingController == null)
                {
                    toppingController = mainPizza.AddComponent<PizzaController>();
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
                */
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

    private GameObject SpawnPizza(Vector3 position, Quaternion rotation, SizeControllerPizza sizeControllerPizza)
    {
        float plateSize = OrderManager.Instance.GetFloatSize() * 2;

        GameObject pizza = Instantiate(
            OrderManager.Instance.currentPizza.arBaseModelPrefab,
            position,
            rotation
        );

        pizza.transform.localScale = Vector3.one * plateSize;

        PizzaController controller = pizza.GetComponent<PizzaController>();
        if (controller == null)
            controller = pizza.AddComponent<PizzaController>();
        sizeControllerPizza.BindToPizza(controller);

        controller.Initialize(plateSize, pizzaLayerMask);


        int ingredientAmount = OrderManager.Instance.GetFloatIngredientAmount();
        foreach (var ingredient in OrderManager.Instance.allIngredients)
        {
            if (OrderManager.Instance.IsIgredientSelected(ingredient))
            {
                controller.SpawnInitialBatch(ingredient, ingredientAmount);
            }
        }
        

        controller.StartPop(Vector3.one * plateSize);
        return pizza;
    }


    public void ComparePizza()
    {
        if (mainPizza == null)
            return;

        if (comparePizza != null)
        {
            Destroy(comparePizza);
            comparePizza = null;
        }

            float distance = 0.45f;
        Vector3 right = mainPizza.transform.right;

        Vector3 leftPosition = mainPizzaOriginalPosition - right * (distance * 0.5f);
        leftPosition.y = mainPizzaOriginalPosition.y;
        mainPizza.transform.position = leftPosition;


        Vector3 rightPosition = mainPizzaOriginalPosition + right * (distance * 0.5f);
        rightPosition.y = mainPizzaOriginalPosition.y;


        comparePizza = SpawnPizza(rightPosition, mainPizza.transform.rotation, compareSizeController);
        PizzaController controller = comparePizza.GetComponent<PizzaController>();
        if (controller == null)
            controller = comparePizza.AddComponent<PizzaController>();

        compareSizeController.BindToPizza(controller);

        OnPizzaCompare.Invoke();

    }




}
