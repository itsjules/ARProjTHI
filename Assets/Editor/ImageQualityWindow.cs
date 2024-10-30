using System.Diagnostics;
using System.IO;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

/// <summary>
/// Image quality window to check image quality using arcoreimg, a command line tool.
/// Require ARCore to be installed using package manager.
/// The recommended score is at least 75.
/// https://developers.google.com/ar/develop/augmented-images/arcoreimg
/// https://developers.google.com/ar/develop/augmented-images#tips_for_selecting_reference_images
/// </summary>
public class ImageQualityWindow : EditorWindow
{
    private XRReferenceImageLibrary imageLibrary;
    private Dictionary<XRReferenceImage, string> qualities = new Dictionary<XRReferenceImage, string>();

    private Vector2 scrollPosition;

    [MenuItem("Tools/Image Quality")]
    public static void ShowWindow()
    {
        GetWindow<ImageQualityWindow>("Image Quality");
    }

    void OnGUI()
    {
        GUILayout.Label("Check image quality for ARCore Android.", EditorStyles.boldLabel);

        imageLibrary = EditorGUILayout.ObjectField("Image Library", imageLibrary, typeof(XRReferenceImageLibrary), false) as XRReferenceImageLibrary;

        if (imageLibrary != null)
        {
            GUILayout.Label("Image Qualities");

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            foreach (XRReferenceImage referenceImage in imageLibrary)
            {
                GUILayout.BeginHorizontal();

                // Check if image.texture is null and display a warning if it is
                if (referenceImage.texture == null)
                {
                    EditorGUILayout.HelpBox("Missing Texture!\nRemember to enable 'Keep Texture at Runtime'", MessageType.Warning);
                }
                else
                {
                    GUILayout.Label(referenceImage.texture, GUILayout.Width(64), GUILayout.Height(64));
                }

                GUILayout.BeginVertical();

                // Display the name
                GUILayout.Label(referenceImage.name);

                // Display the quality
                GUILayout.Label(GetImageQuality(referenceImage));

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            
            if (GUILayout.Button("Check Images Quality"))
            {
                CalculateImagesQuality();
            }
        }
    }

    private string GetImageQuality(XRReferenceImage referenceImage)
    {
        if (qualities.TryGetValue(referenceImage, out string quality))
        {
            return quality;
        }
        return "N/A";
    }

    public void CalculateImagesQuality()
    {
        string folderPath = "Packages/com.unity.xr.arcore/Tools~/";
#if UNITY_EDITOR_WIN
        string fullPath = Path.GetFullPath(folderPath + "Windows/");
#elif UNITY_EDITOR_OSX
        string fullPath = Path.GetFullPath(folderPath + "MacOS/");
#endif

        if (!Directory.Exists(fullPath))
        {
            UnityEngine.Debug.LogError("Could not find arcoreimg tool folder. Please ensure ARCore is installed from package manager.");
            return;
        }

        foreach (XRReferenceImage referenceImage in imageLibrary)
        {
            if (referenceImage.texture != null)
            {
                string picPath = Path.Combine(Environment.CurrentDirectory, AssetDatabase.GetAssetPath(referenceImage.texture));
#if UNITY_EDITOR_WIN
                string argument = "arcoreimg.exe";
#elif UNITY_EDITOR_OSX
                string argument = "./arcoreimg";
#endif
                argument += " eval-img --input_image_path=" + picPath;
                string output = Execute(fullPath + argument);
                qualities[referenceImage] = output;
            }
        }
    }

    string Execute(string command)
    {
        string output = "N/A";
        try
        {
            // Use command line
#if UNITY_EDITOR_WIN
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
#elif UNITY_EDITOR_OSX
        ProcessStartInfo processInfo = new ProcessStartInfo("/bin/bash", " -c \"" + command + " \"");
#endif
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;

            Process process = Process.Start(processInfo);
            output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Dispose();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error executing arcoreimg: " + e.Message);
        }
        return output;
    }
}
