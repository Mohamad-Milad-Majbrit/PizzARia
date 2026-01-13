using UnityEngine;
using UnityEngine.UI;

public class ToggleBackground : MonoBehaviour
{
    [Header("Components")]
    public Image background;
    public Text label; 

    [Header("Background Colors")]
    public Color activeColor = new Color(0.471f, 0.922f, 0.529f, 1.000f); 
    public Color inactiveColor = Color.white;

    [Header("Text Colors")]
    public Color activeTextColor = Color.white;  
    public Color inactiveTextColor = new Color(0.44f, 0.44f, 0.44f, 1.000f); 

    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateVisual);

        UpdateVisual(toggle.isOn);
    }

    void UpdateVisual(bool isOn)
    {

        if (background != null)
        {
            background.color = isOn ? activeColor : inactiveColor;
        }

        if (label != null)
        {
            label.color = isOn ? activeTextColor : inactiveTextColor;
        }
    }
}