using UnityEngine;
using TMPro;  

public class LogToUI : MonoBehaviour
{
    public TMP_Text uiText;  

    
    private const int MaxLogCount = 15;  // Maximum number of logs to display at once
    private string[] logMessages = new string[MaxLogCount];  // Array to hold log messages
    private int logIndex = 0;  // Index to track the current log position

    private void OnEnable()
    {
        // Hook into the log message event
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        // Unhook from the event to avoid memory leaks
        Application.logMessageReceived -= HandleLog;
    }


     private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Format the log message (you can customize this as needed)
        string logMessage = $"{System.DateTime.Now:HH:mm:ss} [{type}] {logString}\n";

        // Add the new log message
        logMessages[logIndex] = logMessage;

        // Increment the index, wrapping it around if needed
        logIndex = (logIndex + 1) % MaxLogCount;

        // Clear the text display
        uiText.text = "";

        // Rebuild the log string from the array
        for (int i = 0; i < MaxLogCount; i++)
        {
            // Only append non-null logs
            if (logMessages[i] != null)
            {
                uiText.text += logMessages[i];
            }
        }
    }
}
