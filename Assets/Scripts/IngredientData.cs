using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Pizza/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public float priceSmall;
    public float priceMedium;
    public float priceLarge;
    public Sprite icon;
    public float kcal;
    public float protein;
    public float carbohydrates;
    public float fat;

    public GameObject arModelPrefab;


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