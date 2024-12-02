using UnityEngine;

public class SlideInAnimation : MonoBehaviour
{
    public RectTransform targetUI; // Das UI-Element, das animiert werden soll
    public Vector2 startPosition; // Startposition der Animation (auﬂerhalb des Bildschirms)
    public Vector2 endPosition; // Zielposition der Animation (sichtbar im Canvas)
    public float duration = 1.0f; // Dauer der Animation in Sekunden

    private float elapsedTime = 0f; // Verstrichene Zeit

    private void Start()
    {
        // Setze die Startposition des UI-Elements
        targetUI.anchoredPosition = startPosition;

        // Starte die Animation
        StartCoroutine(SlideIn());
    }

    private System.Collections.IEnumerator SlideIn()
    {
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Normalisiere die Zeit zwischen 0 und 1
            float t = elapsedTime / duration;

            // Wende Ease-In-Ease-Out auf die interpolierte Zeit an
            t = EaseInOutCubic(t);

            // Setze die interpolierte Position
            targetUI.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            yield return null; // Warte bis zum n‰chsten Frame
        }

        // Stelle sicher, dass die Zielposition exakt erreicht wird
        targetUI.anchoredPosition = endPosition;
    }

    // Ease-In-Out-Funktion (Cubic)
    private float EaseInOutCubic(float t)
    {
        if (t < 0.5f)
            return 4f * t * t * t; // Ease-In
        return 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f; // Ease-Out
    }
}
