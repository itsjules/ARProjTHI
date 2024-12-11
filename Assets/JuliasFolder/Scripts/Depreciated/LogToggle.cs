using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace

public class LogToggle : MonoBehaviour
{
    
    public GameObject logPanel;
    
    public Button toggleButton;
    
    public TMP_Text buttonText;

    private bool isLogPanelVisible;

    public void Start()
    {
    
        isLogPanelVisible= logPanel.activeSelf;
        toggleButton.onClick.AddListener(ToggleLogPanel);
        
        UpdateButtonLabel();
    }

    private void ToggleLogPanel()
    {
    
        isLogPanelVisible = !isLogPanelVisible;
        
        logPanel.SetActive(isLogPanelVisible);

        UpdateButtonLabel();
    }

    private void UpdateButtonLabel()
    {
        
        if (isLogPanelVisible)
        {
            buttonText.text = "Hide Logs";  
        }
        else
        {
            buttonText.text = "Show Logs";  
        }
        
    }
}
