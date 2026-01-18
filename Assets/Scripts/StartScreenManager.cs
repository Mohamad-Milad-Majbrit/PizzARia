using UnityEngine;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the ScreenManager to transition to pizza menu")]
    public ScreenManager screenManager;

    [Header("Settings")]
    [Tooltip("Delay before showing the start screen (useful for initialization)")]
    public float startDelay = 0.1f;
    [Tooltip("Resources path for the photo shown on the start screen")]
    public string photoResourcePath = "StartScreen/startphoto";

    private GameObject startScreenCanvas;
    private Button startButton;

    private void Start()
    {
        Invoke(nameof(InitializeStartScreen), startDelay);
    }

    private void InitializeStartScreen()
    {
        Debug.Log($"StartScreenManager: Building start screen. Photo path='{photoResourcePath}'");
        startScreenCanvas = StartScreenBuilder.CreateStartScreenCanvas(transform, photoResourcePath);

        startButton = startScreenCanvas.GetComponentInChildren<Button>();
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        else
        {
            Debug.LogError("StartScreenManager: Could not find start button in the generated UI!");
        }

        Debug.Log("Start screen created successfully!");
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start button clicked - transitioning to pizza menu");

        if (startScreenCanvas != null)
        {
            startScreenCanvas.SetActive(false);
        }

        if (screenManager != null)
        {
            screenManager.ShowPizzaMenuScreen();
        }
        else
        {
            Debug.LogError("StartScreenManager: ScreenManager reference is not set!");
        }
    }

    public void ShowStartScreen()
    {
        if (startScreenCanvas != null)
        {
            startScreenCanvas.SetActive(true);
        }
    }

    public void HideStartScreen()
    {
        if (startScreenCanvas != null)
        {
            startScreenCanvas.SetActive(false);
        }
    }
}
