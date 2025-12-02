using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Pizza/Ingredient")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public float priceSmall;
    public float priceMedium;
    public float priceLarge;
    public Sprite icon;

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
}