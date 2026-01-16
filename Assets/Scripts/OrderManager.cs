using UnityEngine;
using System.Collections.Generic;
using System;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public List<PizzaData> allPizzas;
    public List<IngredientData> allIngredients;

    public PizzaData currentPizza;
    public int mainSizeIndex = 1;
    public int compareSizeIndex = 1;
    public List<IngredientData> selectedExtraIngredients = new List<IngredientData>();

    public Action OnOrderChanged;
    public event Action<IngredientData, bool> OnIngredientToggled;

    // (Bei dir scheinen das AR-Scale Werte zu sein, NICHT Nutrition)
    public float sizeS = 0.04f;
    public float sizeM = 0.17f;
    public float sizeL = 0.30f;

    // Bei dir: vermutlich "max extra ingredients" oder ähnliches
    public int amountS = 3;
    public int amountM = 4;
    public int amountL = 5;

    public bool showNutritionalValues = false;
    public event Action<bool> OnShowNutritionalValuesChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (allPizzas.Count > 0) SelectPizza(allPizzas[0]);
    }

    public void SelectPizza(PizzaData pizza)
    {
        currentPizza = pizza;
        selectedExtraIngredients.Clear();
        mainSizeIndex = 1;
        UpdateOrder();
    }

    public void SetSizePizzaRole(PizzaRole pizzaRole, int sizeIndex)
    {
        if (pizzaRole == PizzaRole.Main)
            mainSizeIndex = sizeIndex;
        else
            compareSizeIndex = sizeIndex;

        UpdateOrder();
    }

    public void SetSize(int sizeIndex)
    {
        mainSizeIndex = sizeIndex;
        UpdateOrder();
    }

    // =========================
    // Size / Amount helpers
    // =========================

    public float GetFloatSize(PizzaRole pizzaRole)
    {
        if (pizzaRole == PizzaRole.Main)
            return GetFloatSize();

        // BUGFIX: hier compareSizeIndex verwenden
        if (compareSizeIndex == 0) return sizeS;
        if (compareSizeIndex == 1) return sizeM;
        return sizeL;
    }

    public float GetFloatSize()
    {
        if (mainSizeIndex == 0) return sizeS;
        if (mainSizeIndex == 1) return sizeM;
        return sizeL;
    }

    public int GetFloatIngredientAmount(PizzaRole pizzaRole)
    {
        if (pizzaRole == PizzaRole.Main)
            return GetFloatIngredientAmount();

        if (compareSizeIndex == 0) return amountS;
        if (compareSizeIndex == 1) return amountM;
        return amountL;
    }

    public int GetFloatIngredientAmount()
    {
        if (mainSizeIndex == 0) return amountS;
        if (mainSizeIndex == 1) return amountM;
        return amountL;
    }

    private int GetSizeIndex(PizzaRole role)
    {
        return role == PizzaRole.Main ? mainSizeIndex : compareSizeIndex;
    }

    // Nutrition-Faktor nach Größe (weil PizzaData nur 1x kcal/protein/... hat)
    private float GetNutritionFactor(int sizeIndex)
    {
        switch (sizeIndex)
        {
            case 0: return 0.85f; // Small
            case 1: return 1.00f; // Medium
            case 2: return 1.15f; // Large
            default: return 1.00f;
        }
    }

    // =========================
    // Ingredient toggles
    // =========================

    public void ToggleIngredient(IngredientData ingredient)
    {
        if (selectedExtraIngredients.Contains(ingredient))
        {
            selectedExtraIngredients.Remove(ingredient);
            OnIngredientToggled?.Invoke(ingredient, false);
        }
        else
        {
            selectedExtraIngredients.Add(ingredient);
            OnIngredientToggled?.Invoke(ingredient, true);
        }

        UpdateOrder();
    }

    public void TogglePizzen(PizzaData pizzaData)
    {
        currentPizza = pizzaData;
        UpdateOrder();
    }

    public bool IsIgredientSelected(IngredientData ingredientData)
    {
        return selectedExtraIngredients.Contains(ingredientData);
    }

    // =========================
    // Price
    // =========================

    public float GetTotalPrice()
    {
        if (currentPizza == null) return 0;

        float basePrice = 0;
        switch (mainSizeIndex)
        {
            case 0: basePrice = currentPizza.priceSmall; break;
            case 1: basePrice = currentPizza.priceMedium; break;
            case 2: basePrice = currentPizza.priceLarge; break;
        }

        float extrasPrice = 0;
        foreach (var ingredient in selectedExtraIngredients)
        {
            switch (mainSizeIndex)
            {
                case 0: extrasPrice += ingredient.priceSmall; break;
                case 1: extrasPrice += ingredient.priceMedium; break;
                case 2: extrasPrice += ingredient.priceLarge; break;
            }
        }

        return basePrice + extrasPrice;
    }

    public float GetPizzaPrice()
    {
        if (currentPizza == null) return 0;

        switch (mainSizeIndex)
        {
            case 0: return currentPizza.priceSmall;
            case 1: return currentPizza.priceMedium;
            case 2: return currentPizza.priceLarge;
            default: return currentPizza.priceMedium;
        }
    }

    // =========================
    // Nutrition (ALL IN ONE)
    // =========================

    private float referenceRadius = 0.5f; // Prefab: 1m Durchmesser => 0.5m Radius

    private float GetAreaFactor(PizzaRole role)
    {
        float currentRadius = GetFloatSize(role); // echte Meter
        return Mathf.Pow(currentRadius / referenceRadius, 2f);
    }

    public NutritionData GetTotalNutrition(PizzaRole role)
    {
        NutritionData total = new NutritionData();
        if (currentPizza == null) return total;

        float areaFactor = GetAreaFactor(role);

        // Base Pizza (flächenbasiert)
        total = total + (currentPizza.GetNutrition() * areaFactor);

        // Extras (ebenfalls flächenbasiert)
        foreach (var ingredient in selectedExtraIngredients)
        {
            total = total + (ingredient.GetNutrition() * areaFactor);
        }

        return total;
    }


    // Optional: Wrapper, falls du manchmal nur einen Wert brauchst
    public float GetTotalFat(PizzaRole role) => GetTotalNutrition(role).fat;
    public float GetTotalKcal(PizzaRole role) => GetTotalNutrition(role).kcal;
    public float GetTotalProtein(PizzaRole role) => GetTotalNutrition(role).protein;
    public float GetTotalCarbs(PizzaRole role) => GetTotalNutrition(role).carbohydrates;

    // =========================
    // Misc
    // =========================

    public string GetPizzaName()
    {
        if (currentPizza == null) return "keine Pizza";
        return currentPizza.pizzaName; // BUGFIX: du hast pizzaName im ScriptableObject
    }

    public void ToggleNutritionalVisibility()
    {
        showNutritionalValues = !showNutritionalValues;
        OnShowNutritionalValuesChanged?.Invoke(showNutritionalValues);
    }

    public void SetNutritionalVisibility(bool value)
    {
        if (showNutritionalValues == value) return;
        showNutritionalValues = value;
        OnShowNutritionalValuesChanged?.Invoke(showNutritionalValues);
    }

    private void UpdateOrder()
    {
        Debug.Log("Neuer Preis: " + GetTotalPrice() + " €");
        OnOrderChanged?.Invoke();
    }
}
