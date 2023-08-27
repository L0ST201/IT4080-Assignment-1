using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenGenerator : MonoBehaviour
{
    private Canvas canvas;
    private GameObject settingsPage;

    private readonly Color DARK_GRAY = new Color(0.05f, 0.05f, 0.05f);

    private void Start()
    {
        InitializeCanvas();
        CreateMainMenu();
        CreateSettingsPage();
    }

    private void InitializeCanvas()
    {
        GameObject canvasObject = CreateUIObject("Canvas", null);
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        Image canvasImage = canvasObject.AddComponent<Image>();
        canvasImage.color = DARK_GRAY;

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
    }

    private void CreateMainMenu()
    {
        CreateText("WELCOME TO THE GAME", new Vector2(0, 200), new Vector2(600, 100), 24, new Color(0, 0.8f, 0));
        CreateText("Ben Armour", new Vector2(0, 100), new Vector2(400, 50), 16, Color.cyan);
        CreateButton("Start", new Vector2(0, -50), OnStartButtonPressed);
        CreateButton("Settings", new Vector2(0, -125), OnSettingsButtonPressed);
        CreateButton("Exit", new Vector2(0, -200), OnExitButtonPressed);
    }

    private void CreateSettingsPage()
    {
        settingsPage = CreateUIObject("SettingsPage", canvas.transform, false);

        CreateText("Settings", new Vector2(0, 200), new Vector2(400, 50), 24, Color.white).transform.SetParent(settingsPage.transform);

        CreateButton("Back", new Vector2(0, -200), OnBackButtonPressed).transform.SetParent(settingsPage.transform);

        CreateText("Volume", new Vector2(0, 100), new Vector2(100, 30), 16, Color.white).transform.SetParent(settingsPage.transform);

        CreateText("Resolution", new Vector2(0, 50), new Vector2(100, 30), 16, Color.white).transform.SetParent(settingsPage.transform);

        CreateText("Fullscreen", new Vector2(0, 0), new Vector2(100, 30), 16, Color.white).transform.SetParent(settingsPage.transform);
    }

    private GameObject CreateUIObject(string name, Transform parent, bool setActive = true)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent);
        obj.AddComponent<RectTransform>();
        obj.SetActive(setActive);
        return obj;
    }

    private GameObject CreateText(string content, Vector2 position, Vector2 size, int fontSize, Color color)
    {
        GameObject textObject = CreateUIObject(content, canvas.transform);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        SetCommonTextProperties(text, content, fontSize, color);
        SetRectTransformProperties(textObject, position, size);
        return textObject;
    }

    private void SetCommonTextProperties(TextMeshProUGUI text, string content, int fontSize, Color color)
    {
        text.text = content;
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = TextAlignmentOptions.Center;
        text.enableWordWrapping = false;
        text.fontStyle = FontStyles.Bold;
    }

    private void SetRectTransformProperties(GameObject obj, Vector2 position, Vector2 size)
    {
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        rectTransform.localPosition = position;
        rectTransform.sizeDelta = size;
    }

    private GameObject CreateButton(string buttonText, Vector2 position, UnityEngine.Events.UnityAction action)
    {
        GameObject buttonObject = CreateUIObject(buttonText + "Button", canvas.transform);
        Button button = buttonObject.AddComponent<Button>();
        button.onClick.AddListener(action);
        Image image = buttonObject.AddComponent<Image>();
        image.color = ColorFromName(buttonText);
        SetRectTransformProperties(buttonObject, position, new Vector2(200, 40));

        GameObject textObject = CreateText(buttonText, Vector3.zero, new Vector2(200, 40), 14, Color.black);
        textObject.transform.SetParent(buttonObject.transform);
        SetRectTransformProperties(textObject, Vector3.zero, new Vector2(200, 40));
        return buttonObject;
    }

    private Color ColorFromName(string name)
    {
        switch (name)
        {
            case "Start": return new Color(0, 0.8f, 0);
            case "Settings": return Color.yellow;
            case "Exit": return Color.red;
            case "Back": return new Color(0.6f, 0.6f, 0.6f);
            default: return Color.white;
        }
    }

    private void OnStartButtonPressed()
    {
        // Code to start the game or load a scene would go here
    }

    private void OnSettingsButtonPressed()
    {
        Debug.Log("Settings button pressed!");
        ToggleSettingsVisibility(true);
    }

    private void OnExitButtonPressed()
    {
        Debug.Log("Exit button pressed!");
        Application.Quit();
    }

    private void OnBackButtonPressed()
    {
        ToggleSettingsVisibility(false);
    }

    private void ToggleSettingsVisibility(bool showSettings)
    {
        settingsPage.SetActive(showSettings);
        foreach (Transform child in canvas.transform)
        {
            if (child.gameObject != settingsPage)
                child.gameObject.SetActive(!showSettings);
        }
    }
}