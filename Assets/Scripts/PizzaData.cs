using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Pizza", menuName = "Pizza/Pizza")]
public class PizzaData : ScriptableObject
{
    public string pizzaName;
    public float priceSmall;
    public float priceMedium;
    public float priceLarge;
    public float kcal;
    public float protein;
    public float carbohydrates;
    public float fat;
    public GameObject arBaseModelPrefab;

    public float GetPriceForSize(int sizeIndex)
    {
        switch (sizeIndex)
        {
            case 0: return priceSmall;
            case 1: return priceMedium;
            case 2: return priceLarge;
            default: return priceMedium;
        }
    }

    public NutritionData GetNutrition()
    {
        return new NutritionData
        {
            kcal = kcal,
            protein = protein,
            carbohydrates = carbohydrates,
            fat = fat
        };
    }


}