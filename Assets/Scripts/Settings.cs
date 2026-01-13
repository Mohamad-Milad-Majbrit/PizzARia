using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject settingsWindow;
    public void OpenSettings()
    {
        settingsWindow.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsWindow.SetActive(false);
    }
}
