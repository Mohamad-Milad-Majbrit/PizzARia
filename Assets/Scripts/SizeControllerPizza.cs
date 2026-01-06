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

        int sizeIndex = OrderManager.Instance.mainSizeIndex;
        if (sizeIndex == 0)
        {
            toggleS.SetIsOnWithoutNotify(true);
        }
        else if (sizeIndex == 1)
        {
            toggleM.SetIsOnWithoutNotify(true);
        }
        else
        {
            toggleL.SetIsOnWithoutNotify(true);
        }

        gameObject.SetActive(true);
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
