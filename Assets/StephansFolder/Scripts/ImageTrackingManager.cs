using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class ImageTrackingManager : MonoBehaviour
{
    private ARTrackedImageManager trackedImageManager;
    public GameObject imageBackground; // Referenz auf Image_Background
    public GameObject imageSuccess; // Referenz auf Image_Success

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            UnityEngine.Debug.Log($"Bild erkannt: {trackedImage.referenceImage.name}");

            if (trackedImage.referenceImage.name == "start_reference") // Der Name des Bildes aus der XR Reference Image Library
            {
                UnityEngine.Debug.Log("Bild erfolgreich getrackt! Zeige Image_Success und wechsle später die Szene.");
                StartCoroutine(HandleSuccess());
            }
        }
    }

    private System.Collections.IEnumerator HandleSuccess()
    {
        // Blende Image_Background aus
        if (imageBackground != null)
        {
            imageBackground.SetActive(false);
        }

        // Füge einen 1-Sekunden-Delay hinzu, bevor Image_Success erscheint
        yield return new WaitForSeconds(1f);

        // Zeige Image_Success und setze die Skalierung auf 0
        if (imageSuccess != null)
        {
            imageSuccess.SetActive(true);
            imageSuccess.transform.localScale = Vector3.zero;

            // Skaliere Image_Success von 0 auf 100 % in 0.8 Sekunden mit Ease-In-Ease-Out
            float elapsedTime = 0f;
            float duration = 0.5f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // Ease-In-Ease-Out hinzufügen
                t = Mathf.SmoothStep(0f, 1f, t);

                imageSuccess.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }

            // Stelle sicher, dass die Skalierung exakt 100 % erreicht wird
            imageSuccess.transform.localScale = Vector3.one;

            // Warte 2 Sekunden vor dem Szenenwechsel
            yield return new WaitForSeconds(2f);

            // Wechsel zur Szene "Introduction"
            SceneManager.LoadScene("Introduction");
        }
    }
}
