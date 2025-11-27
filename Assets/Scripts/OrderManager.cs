using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public List<PizzaData> allPizzas;
    public List<IngredientData> allIngredients;


    public PizzaData currentPizza;
    public int currentSizeIndex; 
    public List<IngredientData> selectedExtraIngredients = new List<IngredientData>();

    public System.Action OnOrderChanged;

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
        currentSizeIndex = 1;
        UpdateOrder();
    }

    public void SetSize(int sizeIndex)
    {
        currentSizeIndex = sizeIndex;
        UpdateOrder();
    }

    public void ToggleIngredient(IngredientData ingredient)
    {
        if (selectedExtraIngredients.Contains(ingredient))
        {
            selectedExtraIngredients.Remove(ingredient);
        }
        else
        {
            selectedExtraIngredients.Add(ingredient);
        }
        UpdateOrder();
    }

    public float GetTotalPrice()
    {
        if (currentPizza == null) return 0;

        float basePrice = 0;
        switch (currentSizeIndex)
        {
            case 0: basePrice = currentPizza.priceSmall; break;
            case 1: basePrice = currentPizza.priceMedium; break;
            case 2: basePrice = currentPizza.priceLarge; break;
        }

        float extrasPrice = 0;
        foreach (var ingredient in selectedExtraIngredients)
        {
            switch (currentSizeIndex)
            {
                case 0: extrasPrice += ingredient.priceSmall; break;
                case 1: extrasPrice += ingredient.priceMedium; break;
                case 2: extrasPrice += ingredient.priceLarge; break;
            }
        }

        return basePrice + extrasPrice;
    }

    private void UpdateOrder()
    {
        Debug.Log("Neuer Preis: " + GetTotalPrice() + " €");
        OnOrderChanged?.Invoke();
    }
}