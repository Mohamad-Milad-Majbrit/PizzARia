using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum PizzaRole
{
    Main,
    Compare
}
public class PizzaController : MonoBehaviour
{
    private Dictionary<IngredientData, List<GameObject>> toppingMap = new Dictionary<IngredientData, List<GameObject>>();

    public PizzaRole pizzaRole { get; private set; }
    public int sizeIndex { get; private set; }
    private float pizzaSize;
    private LayerMask layerMask;

    public ParticleSystem startSteam;

    public GameObject plate;

    public GameObject pizza;

    public TextMeshPro priceText;
    public GameObject priceTagContainer;


    public float duration = 0.6f; 
    public AnimationCurve animationCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(0.7f, 1.1f), 
        new Keyframe(1, 1)        
    );

    public void SetSizeByIndex(int index)
    {
        foreach (var kvp in toppingMap)
        {
            IngredientData ingredient = kvp.Key;

            foreach (var obj in kvp.Value)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        sizeIndex = index;


        float scale = OrderManager.Instance.GetFloatSize(pizzaRole) * 2;

        pizzaSize = scale;

        pizza.transform.localScale = Vector3.one * scale;

        int ingredientAmount = OrderManager.Instance.GetFloatIngredientAmount(pizzaRole);
        foreach (var ingredient in OrderManager.Instance.allIngredients)
        {
            if (OrderManager.Instance.IsIgredientSelected(ingredient))
            {
                SpawnInitialBatch(ingredient, ingredientAmount);
            }
        }
    }

    public void SetPlateColor(Color color)
    {
        if (plate == null) return;

        Renderer plateRenderer = plate.GetComponent<Renderer>();
        if (plateRenderer == null)
            plateRenderer = plate.GetComponentInChildren<Renderer>();

        if (plateRenderer != null)
        {
            plateRenderer.material.color = color;
        }
    }

    public void StartPop(Vector3 finalScale)
    {
        StartCoroutine(AnimatePop(finalScale));
    }

    private IEnumerator AnimatePop(Vector3 targetScale)
    {
        float timer = 0f;

        pizza.transform.localScale = Vector3.zero;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            float curveValue = animationCurve.Evaluate(progress);

            pizza.transform.localScale = targetScale * curveValue;

            yield return null;
        }

        pizza.transform.localScale = targetScale;
    }
    public void Initialize(float size, LayerMask mask)
    {
        pizza.transform.localScale = Vector3.one * size;
        this.pizzaSize = size;
        this.layerMask = mask;
        startSteam.Play();
    }

    private void Start()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnIngredientToggled += HandleIngredientToggle;
        }
    }

    private void OnDestroy()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnIngredientToggled -= HandleIngredientToggle;
        }
    }

    public void RegisterTopping(IngredientData data, GameObject toppingObj)
    {
        if (!toppingMap.ContainsKey(data))
            toppingMap[data] = new List<GameObject>();

        toppingMap[data].Add(toppingObj);
    }

    private void HandleIngredientToggle(IngredientData data, bool isOn)
    {
        if (toppingMap.ContainsKey(data) && toppingMap[data].Count > 0)
        {
            foreach (GameObject obj in toppingMap[data])
            {
                if (obj != null) obj.SetActive(isOn);
            }
        }
        else if (isOn)
        {
            int amount = OrderManager.Instance.GetFloatIngredientAmount(); 

            StartCoroutine(SpawnSingleIngredientType(data, amount));
        }
    }

    public void SpawnInitialBatch(IngredientData data, int amount)
    {
        StartCoroutine(SpawnSingleIngredientType(data, amount));
    }

    private IEnumerator SpawnSingleIngredientType(IngredientData data, int count)
    {

        toppingMap[data] = new List<GameObject>();

        float normalizedRadius = 0.35f;


        float angleOffset = Random.Range(0f, 2f * Mathf.PI);

        for (int i = 0; i < count; i++)
        {
            // Fibonacci spiral
            float r = normalizedRadius * Mathf.Sqrt((float)i / count);
            float theta = i * Mathf.PI * (3f - Mathf.Sqrt(5f)) + angleOffset; 

            float x = r * Mathf.Cos(theta);
            float z = r * Mathf.Sin(theta);

            Vector3 rayStartLocal = new Vector3(x, 0.2f, z);
            Vector3 rayStartWorld = pizza.transform.TransformPoint(rayStartLocal);

            if (Physics.Raycast(rayStartWorld, -pizza.transform.up, out RaycastHit hitInfo, 0.5f, layerMask))
            {
                GameObject newIngredient = Instantiate(data.arModelPrefab, hitInfo.point, Quaternion.identity);


                newIngredient.transform.SetParent(pizza.transform, true);


                Vector3 originalScale = data.arModelPrefab.transform.localScale;

                newIngredient.transform.localScale = new Vector3(
                    originalScale.x / pizzaSize,
                    originalScale.y / pizzaSize,
                    originalScale.z / pizzaSize
                );


                newIngredient.transform.up = hitInfo.normal;
                newIngredient.transform.Rotate(0, Random.Range(0, 360), 0, Space.Self);

                toppingMap[data].Add(newIngredient);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}