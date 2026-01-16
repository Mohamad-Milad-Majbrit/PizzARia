using UnityEngine;
using TMPro;

public class PizzaDisplay : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI pizzaNameText;

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

        float price = OrderManager.Instance.GetPizzaPrice();

        priceText.text = "" + price.ToString("0.00") + " €";

        pizzaNameText.text = "Pizza " + OrderManager.Instance.GetPizzaName();
    }
}