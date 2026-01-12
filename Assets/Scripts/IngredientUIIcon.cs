using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUIIcon : MonoBehaviour
{

    public Toggle selectionToggle;
    public Image background;

    public SVGImage icon;

    private Color color = new Color(0.471f, 0.922f, 0.529f, 1.000f);

    private Color iconColor = new Color(0.44f, 0.44f, 0.44f, 1.000f);

    private IngredientData myData;



    public void Setup(IngredientData data)
    {
        myData = data;

        selectionToggle.onValueChanged.RemoveAllListeners();
        selectionToggle.onValueChanged.AddListener(OnToggleChanged);

        icon.sprite = data.icon;
        icon.color = iconColor;
        if (OrderManager.Instance.IsIgredientSelected(myData))
        {
            background.color = color;
            icon.color = Color.white;
            
            selectionToggle.SetIsOnWithoutNotify(true);
        }
    }


    void OnToggleChanged(bool isOn)
    {
        OrderManager.Instance.ToggleIngredient(myData);

        if (isOn)
        {
            background.color = color;
            icon.color = Color.white;
        }
        else
        {
            background.color = Color.white;
            icon.color = iconColor;
        }
    }
}
