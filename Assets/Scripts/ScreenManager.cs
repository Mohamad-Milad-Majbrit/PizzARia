using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    [Header("Canvas References")]
    public GameObject ingredientCanvas;       
    public GameObject arCanvas;         

    [Header("Managers")]
    public PlaceOnPlane arManager;
    public UIManager uiManager;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ingredientCanvas.SetActive(true);
        arCanvas.SetActive(false);
        uiManager.GenerateIngredientList();
    }

    public void ShowARScreen()
    {
        ingredientCanvas.SetActive(false);
        arCanvas.SetActive(true);
        arManager.StartARPlacement();
        uiManager.GenerateIngredientIconList();
    }
}
