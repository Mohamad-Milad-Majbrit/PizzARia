using UnityEngine;
using UnityEngine.UI;

public class SizeControllerPizza : MonoBehaviour
{
    public Toggle toggleS;
    public Toggle toggleM;
    public Toggle toggleL;

    private PizzaController currentPizza;
    private bool suppressCallback;

    private void Awake()
    {
        toggleS.onValueChanged.AddListener(isOn => OnSizeChanged(0, isOn));
        toggleM.onValueChanged.AddListener(isOn => OnSizeChanged(1, isOn));
        toggleL.onValueChanged.AddListener(isOn => OnSizeChanged(2, isOn));
    }

    public void BindToPizza(PizzaController pizza)
    {
        currentPizza = pizza;

        // WICHTIG: Hier keine Toggles setzen! Das GameObject ist zu diesem 
        // Zeitpunkt oft noch inaktiv, was die ToggleGroup von Unity durcheinanderbringt.
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (currentPizza != null)
        {
            suppressCallback = true;

            int sizeIndex = (currentPizza.pizzaRole == PizzaRole.Main)
                ? OrderManager.Instance.mainSizeIndex
                : OrderManager.Instance.compareSizeIndex;

            toggleS.isOn = (sizeIndex == 0);
            toggleM.isOn = (sizeIndex == 1);
            toggleL.isOn = (sizeIndex == 2);

            suppressCallback = false;
        }
    }

    private void OnSizeChanged(int sizeIndex, bool isOn)
    {
        if (!isOn || suppressCallback || currentPizza == null)
            return;

        OrderManager.Instance.SetSizePizzaRole(currentPizza.pizzaRole, sizeIndex);
        currentPizza.SetSizeByIndex(sizeIndex);
    }

    public void Hide()
    {
        currentPizza = null;
        gameObject.SetActive(false);
    }
}