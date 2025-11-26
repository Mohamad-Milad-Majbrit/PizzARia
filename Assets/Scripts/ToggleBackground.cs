using UnityEngine;
using UnityEngine.UI;

public class ToggleBackground : MonoBehaviour
{
    public Image background;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;

    private Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateVisual);
        UpdateVisual(toggle.isOn);
    }

    void UpdateVisual(bool isOn)
    {
        background.color = isOn ? activeColor : inactiveColor;
    }
}
