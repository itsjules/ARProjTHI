using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StickerDownload : MonoBehaviour
{
    public Button downloadButton; // Referenz zum Button

    private void Start()
    {
        // Button-Click-Event hinzuf�gen
        if (downloadButton != null)
        {
            downloadButton.onClick.AddListener(DownloadSticker);
        }
    }

    private void DownloadSticker()
    {
        string fileName = "StickerTHI.webp";

        // Absoluter Pfad zur Datei im StreamingAssets-Ordner
        string sourcePath = Path.Combine(UnityEngine.Application.dataPath, "Stephansfolder/StreamingAssets", fileName);

        // Zielpfad im lokalen Downloads-Verzeichnis
        string destinationPath = Path.Combine(UnityEngine.Application.persistentDataPath, fileName);

        if (File.Exists(sourcePath))
        {
            // Datei kopieren
            File.Copy(sourcePath, destinationPath, true);

            // Debug-Nachricht f�r den Nutzer
            UnityEngine.Debug.Log($"Sticker wurde heruntergeladen: {destinationPath}");

            // Optional: �ffne den Ordner (plattformabh�ngig)
#if UNITY_EDITOR
            UnityEngine.Application.OpenURL("file://" + destinationPath);
#elif UNITY_ANDROID || UNITY_IOS
            UnityEngine.Application.OpenURL(destinationPath);
#endif
        }
        else
        {
            UnityEngine.Debug.LogError("Die Datei konnte nicht gefunden werden: " + sourcePath);
        }
    }
}
