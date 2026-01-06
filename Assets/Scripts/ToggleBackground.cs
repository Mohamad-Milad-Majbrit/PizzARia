using UnityEngine;
using UnityEngine.UI;

public class ToggleBackground : MonoBehaviour
{
    public Image background;
    public Color activeColor = new Color(0.471f, 0.922f, 0.529f, 1.000f);
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
