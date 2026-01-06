
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

    public GameObject sizeToggleMain;
    public GameObject sizeToggleCompare;

    private void Start()
    {

        sizeToggleMain.SetActive(false);
        sizeToggleCompare.SetActive(false);
    }


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


    /*public void ShowSizeToggles()
    {
        sizeToggleMain.SetActive(true);
        sizeToggleCompare.SetActive(true);

        int sizeIndex = OrderManager.Instance.mainSizeIndex;
        if (sizeIndex == 0)
        {
            Toggle toggleS = sizeToggleMain.transform.Find("ToggleS").GetComponent<Toggle>();
            toggleS.SetIsOnWithoutNotify(true);
        }
        else if (sizeIndex == 1)
        {
            Toggle toggleM = sizeToggleMain.transform.Find("ToggleM").GetComponent<Toggle>();
            toggleM.SetIsOnWithoutNotify(true);
        }
        else
        {
            Toggle toggleL = sizeToggleMain.transform.Find("ToggleL").GetComponent<Toggle>();
            toggleL.SetIsOnWithoutNotify(true);
        }
        
    }*/

}
