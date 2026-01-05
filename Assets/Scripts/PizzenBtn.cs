using UnityEngine;

public class PizzenBtn : MonoBehaviour
{
    public PizzaData pizzaData;
   
    public void SelectPizza()
    {
        OrderManager.Instance.TogglePizzen(pizzaData);
        ScreenManager.Instance.ShowIngredientScreen();
    }
}
