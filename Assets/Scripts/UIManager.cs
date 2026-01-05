
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform contentArea; 
    public GameObject ingredientPrefab;
    public Transform contentIconArea;
    public GameObject ingredientIconPrefab;

    public GameObject showIngredientsButton;
    public GameObject ingredientIcons;


    public void GenerateIngredientList()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }

        foreach (var ingredient in OrderManager.Instance.allIngredients)
        {
            GameObject newRow = Instantiate(ingredientPrefab, contentArea);

            IngredientUIItem script = newRow.GetComponent<IngredientUIItem>();
            script.Setup(ingredient);
        }
    }

    public void GenerateIngredientIconList()
    {
        foreach (Transform child in contentIconArea)
        {
            Destroy(child.gameObject);
        }

        foreach (var ingredient in OrderManager.Instance.allIngredients)
        {
            GameObject newRow = Instantiate(ingredientIconPrefab, contentIconArea);

            IngredientUIIcon script = newRow.GetComponent<IngredientUIIcon>();
            script.Setup(ingredient);

        }
        showIngredientsButton.SetActive(false);
        ingredientIcons.SetActive(false);
    }

    public void ShowIngredientButton()
    {
        showIngredientsButton.SetActive(true);
    }


}
