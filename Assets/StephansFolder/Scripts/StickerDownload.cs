using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;

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

// #if UNITY_ANDROID
//         // Zielpfad zum Android Downloads-Verzeichnis
//         string destinationPath = Path.Combine("/storage/emulated/0/Download", fileName);

// #elif UNITY_EDITOR
        // Zielpfad im lokalen Downloads-Verzeichnis
        string destinationPath = Path.Combine(UnityEngine.Application.persistentDataPath, fileName);
// #endif

         StartCoroutine(CopyFileFromStreamingAssets(sourcePath, destinationPath));

    }

    private System.Collections.IEnumerator CopyFileFromStreamingAssets(string sourcePath, string destinationPath)
    {
        UnityWebRequest request = UnityWebRequest.Get(sourcePath);

        // Datei laden
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                // Datei im Zielverzeichnis speichern
                File.WriteAllBytes(destinationPath, request.downloadHandler.data);

                // Debug-Nachricht für den Nutzer
                Debug.Log($"Sticker was downlaoded: {destinationPath}");

                // Optional: Öffne den Ordner (plattformabhängig)
#if UNITY_EDITOR
                Application.OpenURL("file://" + destinationPath);
#elif UNITY_ANDROID 
                Application.OpenURL(destinationPath);
#endif
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error with saving Sticker: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Sticker couldn't be loaded: " + request.error);
        }
    }
}
