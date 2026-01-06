using UnityEngine;
using System.Collections.Generic;
using System;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public List<PizzaData> allPizzas;
    public List<IngredientData> allIngredients;


    public PizzaData currentPizza;
    public int mainSizeIndex =1; 
    public int compareSizeIndex =1;
    public List<IngredientData> selectedExtraIngredients = new List<IngredientData>();

    public System.Action OnOrderChanged;
    public event Action<IngredientData, bool> OnIngredientToggled;

    public float sizeS = 0.04f;//0.14f;
    public float sizeM = 0.17f;
    public float sizeL = 0.30f; //0.19f;

    public int amountS = 3;
    public int amountM = 4;
    public int amountL = 5;

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
        if(pizzaRole == PizzaRole.Main)
        {
            mainSizeIndex = sizeIndex;
        }
        else
        {
            compareSizeIndex = sizeIndex;
        }

    }

    public void SetSize(int sizeIndex)
    {
        mainSizeIndex = sizeIndex;
        UpdateOrder();
    }


    public float GetFloatSize(PizzaRole pizzaRole)
    {

        if (pizzaRole == PizzaRole.Main)
        {
            return GetFloatSize();
        }
        else
        {
            if (mainSizeIndex == 0)
            {
                return sizeS;
            }
            else if (mainSizeIndex == 1)
            {
                return sizeM;
            }
            else
            {
                return sizeL;
            }
        }
    }
    public float GetFloatSize()
    {
        if (mainSizeIndex == 0)
        {
            return sizeS;
        }
        else if (mainSizeIndex == 1)
        {
            return sizeM;
        }
        else
        {
            return sizeL;
        }
    }


    public int GetFloatIngredientAmount(PizzaRole pizzaRole)
    {
        if(pizzaRole == PizzaRole.Main)
        {
            return GetFloatIngredientAmount();
        } else
        {
            if (compareSizeIndex == 0)
            {
                return amountS;
            }
            else if (compareSizeIndex == 1)
            {
                return amountM;
            }
            else
            {
                return amountL;
            }
        }

    }
    public int GetFloatIngredientAmount()
    {
        if (mainSizeIndex == 0)
        {
            return amountS;
        }
        else if (mainSizeIndex == 1)
        {
            return amountM;
        }
        else
        {
            return amountL;
        }
    }

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

    }

    public bool IsIgredientSelected(IngredientData ingredientData)
    {
        if (selectedExtraIngredients.Contains(ingredientData))
        {
            return true;
        }
        return false;
    }
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

        float basePrice = 0;
        switch (mainSizeIndex)
        {
            case 0: basePrice = currentPizza.priceSmall; break;
            case 1: basePrice = currentPizza.priceMedium; break;
            case 2: basePrice = currentPizza.priceLarge; break;
        }
        return basePrice ;
    }

    public string GetPizzaName()
    {
        if (currentPizza == null) return "keine Pizza";
        return currentPizza.name;

    }


    private void UpdateOrder()
    {
        Debug.Log("Neuer Preis: " + GetTotalPrice() + " €");
        OnOrderChanged?.Invoke();
    }
}