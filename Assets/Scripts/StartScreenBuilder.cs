using UnityEngine;
using UnityEngine.UI;
using TMPro;


public static class StartScreenBuilder
{
    private static readonly Color bgWhite = new Color(0.98f, 0.98f, 0.98f); // Off-white #FAFAFA
    private static readonly Color primaryGreen = new Color(0x2E / 255f, 0x7D / 255f, 0x32 / 255f); // Deep green #2E7D32
    private static readonly Color lightGreen = new Color(0x4C / 255f, 0xAF / 255f, 0x50 / 255f); // Material green #4CAF50
    private static readonly Color darkText = new Color(0x21 / 255f, 0x21 / 255f, 0x21 / 255f); // #212121
    private static readonly Color grayText = new Color(0x75 / 255f, 0x75 / 255f, 0x75 / 255f); // #757575
    private static readonly Color lightGray = new Color(0.92f, 0.92f, 0.92f); // #EBEBEB

    private const string DefaultPhotoResourcePath = "StartScreen/startphoto";

    public static GameObject CreateStartScreenCanvas(Transform parent = null, string photoResourcePath = null)
    {
        GameObject canvasObj = new GameObject("StartScreenCanvas");
        if (parent != null) canvasObj.transform.SetParent(parent);

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();

        CreateBackground(canvasObj.transform);

        GameObject content = CreateContentArea(canvasObj.transform);

        CreateTitle(content.transform);

        Sprite photoSprite = TryLoadPhotoSprite(photoResourcePath);
        if (photoSprite != null)
        {
            CreatePhotoCard(content.transform, photoSprite);
        }

        CreateSubtitle(content.transform);

        CreateButton(content.transform);

        return canvasObj;
    }

    private static void CreateBackground(Transform parent)
    {
        GameObject bgBottom = new GameObject("BackgroundBottom");
        bgBottom.transform.SetParent(parent, false);

        Image bottomImg = bgBottom.AddComponent<Image>();
        bottomImg.color = new Color(0.96f, 0.93f, 0.88f); 
        bottomImg.raycastTarget = false;

        RectTransform bottomRect = bgBottom.GetComponent<RectTransform>();
        bottomRect.anchorMin = Vector2.zero;
        bottomRect.anchorMax = Vector2.one;
        bottomRect.sizeDelta = Vector2.zero;

        GameObject bgTop = new GameObject("BackgroundTop");
        bgTop.transform.SetParent(parent, false);

        Image topImg = bgTop.AddComponent<Image>();
        topImg.color = new Color(1f, 1f, 1f); 
        topImg.raycastTarget = false;

        UIGradient gradient = bgTop.AddComponent<UIGradient>();

        RectTransform topRect = bgTop.GetComponent<RectTransform>();
        topRect.anchorMin = Vector2.zero;
        topRect.anchorMax = Vector2.one;
        topRect.sizeDelta = Vector2.zero;

        GameObject circle = new GameObject("DecorCircle");
        circle.transform.SetParent(parent, false);

        Image circleImg = circle.AddComponent<Image>();
        circleImg.color = new Color(0.30f, 0.69f, 0.31f, 0.08f);
        circleImg.raycastTarget = false;

        RectTransform circleRect = circle.GetComponent<RectTransform>();
        circleRect.anchorMin = new Vector2(1, 1);
        circleRect.anchorMax = new Vector2(1, 1);
        circleRect.pivot = new Vector2(1, 1);
        circleRect.sizeDelta = new Vector2(400, 400);
        circleRect.anchoredPosition = new Vector2(100, 100);

        // Second decorative circle bottom left
        GameObject circle2 = new GameObject("DecorCircle2");
        circle2.transform.SetParent(parent, false);

        Image circle2Img = circle2.AddComponent<Image>();
        circle2Img.color = new Color(0.30f, 0.69f, 0.31f, 0.06f);
        circle2Img.raycastTarget = false;

        RectTransform circle2Rect = circle2.GetComponent<RectTransform>();
        circle2Rect.anchorMin = Vector2.zero;
        circle2Rect.anchorMax = Vector2.zero;
        circle2Rect.pivot = Vector2.zero;
        circle2Rect.sizeDelta = new Vector2(300, 300);
        circle2Rect.anchoredPosition = new Vector2(-80, -80);
    }

    private static GameObject CreateContentArea(Transform parent)
    {
        GameObject content = new GameObject("Content");
        content.transform.SetParent(parent, false);

        RectTransform rect = content.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(60, 100);
        rect.offsetMax = new Vector2(-60, -100);

        VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 50;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        return content;
    }

    private static void CreateTitle(Transform parent)
    {
        GameObject titleArea = new GameObject("TitleArea");
        titleArea.transform.SetParent(parent, false);

        RectTransform areaRect = titleArea.AddComponent<RectTransform>();
        areaRect.sizeDelta = new Vector2(600, 180);

        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(titleArea.transform, false);

        TextMeshProUGUI title = titleObj.AddComponent<TextMeshProUGUI>();
        title.text = "PizzARia";
        title.fontSize = 126;
        title.fontStyle = FontStyles.Bold;
        title.alignment = TextAlignmentOptions.Center;
        title.color = darkText;
        title.raycastTarget = false;

        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.4f);
        titleRect.anchorMax = Vector2.one;
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        GameObject line = new GameObject("Accent");
        line.transform.SetParent(titleArea.transform, false);

        Image lineImg = line.AddComponent<Image>();
        lineImg.color = lightGreen;
        lineImg.raycastTarget = false;

        RectTransform lineRect = line.GetComponent<RectTransform>();
        lineRect.anchorMin = new Vector2(0.2f, 0.35f);
        lineRect.anchorMax = new Vector2(0.8f, 0.38f);
        lineRect.offsetMin = Vector2.zero;
        lineRect.offsetMax = Vector2.zero;

        GameObject tagObj = new GameObject("Tagline");
        tagObj.transform.SetParent(titleArea.transform, false);

        TextMeshProUGUI tag = tagObj.AddComponent<TextMeshProUGUI>();
        tag.text = "Erstelle deine perfekte Pizza in AR";
        tag.fontSize = 36;
        tag.alignment = TextAlignmentOptions.Center;
        tag.color = grayText;
        tag.raycastTarget = false;

        RectTransform tagRect = tagObj.GetComponent<RectTransform>();
        tagRect.anchorMin = Vector2.zero;
        tagRect.anchorMax = new Vector2(1, 0.3f);
        tagRect.offsetMin = Vector2.zero;
        tagRect.offsetMax = Vector2.zero;
    }

    private static void CreatePhotoCard(Transform parent, Sprite sprite)
    {
        GameObject card = new GameObject("PhotoCard");
        card.transform.SetParent(parent, false);

        RectTransform cardRect = card.AddComponent<RectTransform>();
        cardRect.sizeDelta = new Vector2(580, 580);

        Image photoImg = card.AddComponent<Image>();
        photoImg.sprite = sprite;
        photoImg.preserveAspect = true;
        photoImg.raycastTarget = false;

        Shadow shadow = card.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.25f);
        shadow.effectDistance = new Vector2(0, -10);

        Shadow shadow2 = card.AddComponent<Shadow>();
        shadow2.effectColor = new Color(0, 0, 0, 0.1f);
        shadow2.effectDistance = new Vector2(0, -4);
    }

    private static void CreateSubtitle(Transform parent)
    {
        GameObject subObj = new GameObject("Subtitle");
        subObj.transform.SetParent(parent, false);

        RectTransform rect = subObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(600, 60);

        TextMeshProUGUI text = subObj.AddComponent<TextMeshProUGUI>();
        text.text = "Anpassen  ·  Vorschau  ·  Bestellen";
        text.fontSize = 36;
        text.alignment = TextAlignmentOptions.Center;
        text.color = grayText;
        text.characterSpacing = 2;
        text.raycastTarget = false;
    }

    private static void CreateButton(Transform parent)
    {
        GameObject btnObj = new GameObject("StartButton");
        btnObj.transform.SetParent(parent, false);

        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(480, 100);

        Image btnBg = btnObj.AddComponent<Image>();
        btnBg.color = primaryGreen;

        Button btn = btnObj.AddComponent<Button>();
        ColorBlock colors = btn.colors;
        colors.normalColor = primaryGreen;
        colors.highlightedColor = lightGreen;
        colors.pressedColor = new Color(0x1B / 255f, 0x5E / 255f, 0x20 / 255f);
        colors.fadeDuration = 0.1f;
        btn.colors = colors;

        Shadow btnShadow = btnObj.AddComponent<Shadow>();
        btnShadow.effectColor = new Color(0x2E / 255f, 0x7D / 255f, 0x32 / 255f, 0.4f);
        btnShadow.effectDistance = new Vector2(0, -4);

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(btnObj.transform, false);

        TextMeshProUGUI btnText = textObj.AddComponent<TextMeshProUGUI>();
        btnText.text = "Start";
        btnText.fontSize = 42;
        btnText.fontStyle = FontStyles.Bold;
        btnText.alignment = TextAlignmentOptions.Center;
        btnText.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
    }

    private static Sprite TryLoadPhotoSprite(string resourcePath)
    {
        string path = string.IsNullOrEmpty(resourcePath) ? DefaultPhotoResourcePath : resourcePath;

        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite != null) return sprite;

        Texture2D tex = Resources.Load<Texture2D>(path);
        if (tex != null)
        {
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        }

        Debug.LogWarning($"StartScreen: Photo not found at '{path}'");
        return null;
    }
}


public class UIGradient : BaseMeshEffect
{
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        var vertices = new System.Collections.Generic.List<UIVertex>();
        vh.GetUIVertexStream(vertices);

        if (vertices.Count == 0) return;

        float bottomY = vertices[0].position.y;
        float topY = vertices[0].position.y;

        foreach (var v in vertices)
        {
            if (v.position.y < bottomY) bottomY = v.position.y;
            if (v.position.y > topY) topY = v.position.y;
        }

        float height = topY - bottomY;
        if (height <= 0) return;

        for (int i = 0; i < vertices.Count; i++)
        {
            var v = vertices[i];
            float t = (v.position.y - bottomY) / height;
            v.color = new Color32(v.color.r, v.color.g, v.color.b, (byte)(255 * t));
            vertices[i] = v;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(vertices);
    }
}
