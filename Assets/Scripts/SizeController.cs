using UnityEngine;
using UnityEngine.UI;

public class SizeController : MonoBehaviour
{
    public Toggle toggleS;
    public Toggle toggleM;
    public Toggle toggleL;

    void Start()
    {
        toggleS.onValueChanged.AddListener(delegate { OnSizeChanged(0); });
        toggleM.onValueChanged.AddListener(delegate { OnSizeChanged(1); });
        toggleL.onValueChanged.AddListener(delegate { OnSizeChanged(2); });
    }

    void OnSizeChanged(int sizeIndex)
    {
        bool isSwitchedOn = false;
        if (sizeIndex == 0 && toggleS.isOn) isSwitchedOn = true;
        if (sizeIndex == 1 && toggleM.isOn) isSwitchedOn = true;
        if (sizeIndex == 2 && toggleL.isOn) isSwitchedOn = true;

        if (isSwitchedOn)
        {
            OrderManager.Instance.SetSize(sizeIndex);
        }
    }
}