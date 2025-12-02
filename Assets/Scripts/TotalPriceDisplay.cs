using UnityEngine;
using TMPro;

public class TotalPriceDisplay : MonoBehaviour
{
    public TextMeshProUGUI priceText; // Dein "TotalPrice"-Text

    void Start()
    {
        OrderManager.Instance.OnOrderChanged += UpdatePriceText;

        UpdatePriceText();
    }

    void OnDestroy()
    {
        if (OrderManager.Instance != null)
        {
            OrderManager.Instance.OnOrderChanged -= UpdatePriceText;
        }
    }

    void UpdatePriceText()
    {
        float totalPrice = OrderManager.Instance.GetTotalPrice();

        priceText.text = totalPrice.ToString("0.00") + " €";
    }
}