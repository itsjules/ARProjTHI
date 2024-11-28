using UnityEngine;
using TMPro;  

public class LogToUI : MonoBehaviour
{
    public TMP_Text uiText;  

    
    private const int MaxLogCount = 15;  // Maximum number of logs to display at once
    private string[] logMessages = new string[MaxLogCount];  // Array to hold log messages
    private int logIndex = 0;  // Index to track the current log position

    public void OnEnable()
    {
        
        Application.logMessageReceived += HandleLog;
    }

    public void OnDisable()
    {
        
        Application.logMessageReceived -= HandleLog;
    }

    


    private void HandleLog(string logString, string stackTrace, LogType type)
    {
     
        string logMessage = $"{System.DateTime.Now:HH:mm:ss} [{type}] {logString}\n";

       
        logMessages[logIndex] = logMessage;

        logIndex = (logIndex + 1) % MaxLogCount;

        uiText.text = "";

        
        for (int i = 0; i < MaxLogCount; i++)
        {
            
            if (logMessages[i] != null)
            {
                uiText.text += logMessages[i];
            }
        }
    }
}
