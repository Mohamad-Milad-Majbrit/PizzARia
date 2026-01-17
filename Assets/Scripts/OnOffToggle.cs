using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class OnOffToggle : MonoBehaviour
{
    [Header("Die Container")]
    public GameObject objectOn;  // Zieh hier das "ToggleOn" GameObject rein
    public GameObject objectOff; // Zieh hier das "ToggleOff" GameObject rein

    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        // Auf Klicks hören
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        // Sofort beim Start den richtigen Zustand setzen
        OnToggleValueChanged(toggle.isOn);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // Hartes Umschalten: Einer an, der andere aus.
        if (objectOn != null) objectOn.SetActive(isOn);
        if (objectOff != null) objectOff.SetActive(!isOn);
    }
}

