using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngredientUIItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Toggle selectionToggle;
    public Image background;
    private Color color = new Color(0.471f, 0.922f, 0.529f, 1.000f);
    private Color textColor = new Color(0.44f, 0.44f, 0.44f, 1.000f);

    private IngredientData myData;


    void Start()
    {

        OrderManager.Instance.OnOrderChanged += UpdatePriceDisplay;
    }

    void OnDestroy()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnOrderChanged -= UpdatePriceDisplay;
        }
    }
    public void Setup(IngredientData data)
    {
        myData = data;
        nameText.text = data.ingredientName;
        priceText.text = "+ " + data.priceMedium.ToString("0.00") + " €";

        selectionToggle.onValueChanged.RemoveAllListeners();
        selectionToggle.onValueChanged.AddListener(OnToggleChanged);

        UpdatePriceDisplay();
    }

    void UpdatePriceDisplay()
    {
        int currentSize = OrderManager.Instance.mainSizeIndex;

        float price = myData.GetPriceForSize(currentSize);

        priceText.text = "" + price.ToString("0.00") + " €";
    }

    void OnToggleChanged(bool isOn)
    {
        OrderManager.Instance.ToggleIngredient(myData);

        if (isOn)
        {
            background.color = color;
            nameText.color = Color.white;
        }
        else
        {
            background.color = Color.white;
            nameText.color = textColor;
        }
    }

}