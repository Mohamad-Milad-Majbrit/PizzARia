
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Comparison Controls")]
    public PlaceOnPlane placeOnPlaneScript;
    public Settings settingsScript;
    public Button compareButton;
    public TextMeshProUGUI compareButtonText;


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
        if (compareButton != null)
        {
            compareButton.interactable = false;
        }
    }

    public void ToggleComparisonMode()
    {
        if (placeOnPlaneScript.isComparing)
        {
            placeOnPlaneScript.StopComparison();
            compareButtonText.text = "Pizzen vergleichen";
            settingsScript.CloseSettings(); 
        }
        else
        {
            placeOnPlaneScript.ComparePizza();
            compareButtonText.text = "Pizzenvergleich stoppen";
            settingsScript.CloseSettings(); 
        }
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

    public void EnableCompareButton()
    {
        if (compareButton != null)
        {
            compareButton.interactable = true;
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
