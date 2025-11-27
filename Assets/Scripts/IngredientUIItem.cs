using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngredientUIItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Toggle selectionToggle;

    private IngredientData myData;

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
        int currentSize = OrderManager.Instance.currentSizeIndex;

        float price = myData.GetPriceForSize(currentSize);

        priceText.text = "" + price.ToString("0.00") + " €";
    }

    void OnToggleChanged(bool isOn)
    {
        OrderManager.Instance.ToggleIngredient(myData);
    }
}