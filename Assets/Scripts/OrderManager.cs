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

    public float sizeS = 0.14f;
    public float sizeM = 0.17f;
    public float sizeL = 0.19f;

    
    public int amountS = 3;
    public int amountM = 4;
    public int amountL = 5;

    public bool showNutritionalValues = false;
    public event Action<bool> OnShowNutritionalValuesChanged;
    public bool showPrice = false;
    public event Action<bool> OnPriceVisibilityChanged;

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
        {
            mainSizeIndex = sizeIndex;
        }
        else { 
            compareSizeIndex = sizeIndex;
        }
        OnOrderChanged.Invoke();

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

        Debug.Log(selectedExtraIngredients.Count);

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



    public float GetTotalPrice(PizzaRole role)
    {
        if (currentPizza == null) return 0;

        int sizeIndexToCheck = (role == PizzaRole.Main) ? mainSizeIndex : compareSizeIndex;

        float basePrice = 0;
        switch (sizeIndexToCheck)
        {
            case 0: basePrice = currentPizza.priceSmall; break;
            case 1: basePrice = currentPizza.priceMedium; break;
            case 2: basePrice = currentPizza.priceLarge; break;
        }

        float extrasPrice = 0;
        foreach (var ingredient in selectedExtraIngredients)
        {
            switch (sizeIndexToCheck)
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

        // Base Pizza (fl�chenbasiert)
        total = total + (currentPizza.GetNutrition() * areaFactor);

        int sizeIndexToCheck = (role == PizzaRole.Main) ? mainSizeIndex : compareSizeIndex;

        foreach (var ingredient in selectedExtraIngredients)
        {

            switch (sizeIndexToCheck)
            {
                case 0: total += ingredient.GetNutrition() * amountS; break;
                case 1: total += ingredient.GetNutrition() * amountM; break;
                case 2: total += ingredient.GetNutrition() * amountL; break;
            }
        }

        return total;
    }


    // Wrapper, falls manchmal nur einen Wert 
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
        return currentPizza.pizzaName; 
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
    public void TogglePriceVisibility()
    {
        showPrice = !showPrice;
        OnPriceVisibilityChanged?.Invoke(showPrice);
    }

    private void UpdateOrder()
    {
        Debug.Log("Neuer Preis: " + GetTotalPrice() + " �");
        OnOrderChanged?.Invoke();
    }
}
