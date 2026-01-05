using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIngredientButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public GameObject ingredientIcons;
    public Image buttonImage;

    private bool areIngredientsVisible = false;

    public void ShowIngredientIcons()
    {
        areIngredientsVisible = !areIngredientsVisible;

        if (areIngredientsVisible)
        {
            if (buttonImage != null) buttonImage.color = Color.green;
            if (buttonText != null) buttonText.text = "Zutaten ausblenden";
            ingredientIcons.SetActive(true);
        }
        else
        {
            if (buttonImage != null) buttonImage.color = Color.white;
            if (buttonText != null) buttonText.text = "Zutaten einblenden";
            ingredientIcons.SetActive(false);
        }
    }
}
