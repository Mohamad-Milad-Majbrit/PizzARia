using UnityEngine;

using System;

[Serializable]
public struct NutritionData
{
    public float kcal;
    public float protein;
    public float carbohydrates;
    public float fat;

    public static NutritionData operator +(NutritionData a, NutritionData b)
    {
        return new NutritionData
        {
            kcal = a.kcal + b.kcal,
            protein = a.protein + b.protein,
            carbohydrates = a.carbohydrates + b.carbohydrates,
            fat = a.fat + b.fat
        };
    }

    public static NutritionData operator *(NutritionData a, float factor)
    {
        return new NutritionData
        {
            kcal = a.kcal * factor,
            protein = a.protein * factor,
            carbohydrates = a.carbohydrates * factor,
            fat = a.fat * factor
        };
    }
}

