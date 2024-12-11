using UnityEngine;
using System.Collections;

public class SlideInOutAnimation : MonoBehaviour
{
    public RectTransform targetUI; // Das UI-Element, das animiert werden soll
    public Vector2 offScreenPosition; // Position außerhalb des Bildschirms
    public Vector2 onScreenPosition; // Zielposition im Canvas
    public float duration = 0.8f; // Dauer der Animation in Sekunden

    private Coroutine currentCoroutine;

    private void OnEnable()
    {
        StartSlideIn();
    }

    // private void OnDisable()
    // {
    //     StartSlideOut();
    // }

    public void StartSlideIn()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Slide(targetUI, offScreenPosition, onScreenPosition, duration));
    }

    public void StartSlideOut()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Slide(targetUI, onScreenPosition, offScreenPosition, duration));
    }

    private IEnumerator Slide(RectTransform rectTransform, Vector2 startPosition, Vector2 endPosition, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

        
            float t = elapsedTime / duration;

            
            t = EaseInOutCubic(t);

           
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            yield return null; 
        }

        rectTransform.anchoredPosition = endPosition;
    }

    // Ease-In-Out-Funktion (Cubic)
    private float EaseInOutCubic(float t)
    {
        if (t < 0.5f)
            return 4f * t * t * t; // Ease-In
        return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f; // Ease-Out
    }
}
