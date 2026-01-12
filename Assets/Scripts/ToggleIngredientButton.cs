using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIngredientButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public GameObject ingredientIcons;
    public Image buttonImage;

    private bool areIngredientsVisible = false;
    private Color color = new Color(0.471f, 0.922f, 0.529f, 1.000f);
    private Color textColor = new Color(0.44f, 0.44f, 0.44f, 1.000f);

    public void ShowIngredientIcons()
    {
        areIngredientsVisible = !areIngredientsVisible;

        if (areIngredientsVisible)
        {
            if (buttonImage != null) buttonImage.color = color;
            if (buttonText != null) buttonText.text = "Zutatenauswahl ausblenden";
            if (buttonText != null) buttonText.color = Color.white;
            ingredientIcons.SetActive(true);
        }
        else
        {
            if (buttonImage != null) buttonImage.color = Color.white;
            if (buttonText != null) buttonText.text = "Zutatenauswahl einblenden";
            if (buttonText != null) buttonText.color = textColor;
            ingredientIcons.SetActive(false);
        }
    }
}
