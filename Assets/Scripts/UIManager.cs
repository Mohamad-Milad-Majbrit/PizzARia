using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform contentArea; 
    public GameObject ingredientPrefab; 

    void Start()
    {
        GenerateIngredientList();
    }

    void GenerateIngredientList()
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
}
