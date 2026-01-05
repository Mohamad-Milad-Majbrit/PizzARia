using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUIIcon : MonoBehaviour
{

    public Toggle selectionToggle;
    public Image background;

    public SVGImage icon;

    private IngredientData myData;



    public void Setup(IngredientData data)
    {
        myData = data;

        selectionToggle.onValueChanged.RemoveAllListeners();
        selectionToggle.onValueChanged.AddListener(OnToggleChanged);

        icon.sprite = data.icon;

        if (OrderManager.Instance.IsIgredientSelected(myData))
        {
            background.color = Color.green;
            selectionToggle.SetIsOnWithoutNotify(true);
        }
    }


    void OnToggleChanged(bool isOn)
    {
        OrderManager.Instance.ToggleIngredient(myData);

        if (isOn)
        {
            background.color = Color.green;
        }
        else
        {
            background.color = Color.white;
        }
    }
}
