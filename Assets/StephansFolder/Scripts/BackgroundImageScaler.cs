using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageScaler : MonoBehaviour
{
    public RectTransform canvasRectTransform;

    private RectTransform imageRectTransform;

    void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();

        float canvasHeight = canvasRectTransform.rect.height;

        float aspectRatio = imageRectTransform.rect.width / imageRectTransform.rect.height;
        imageRectTransform.sizeDelta = new Vector2(canvasHeight * aspectRatio, canvasHeight);

        imageRectTransform.anchorMin = new Vector2(1, 0);
        imageRectTransform.anchorMax = new Vector2(1, 0);
        imageRectTransform.pivot = new Vector2(1, 0);
        imageRectTransform.anchoredPosition = Vector2.zero;
    }
}
