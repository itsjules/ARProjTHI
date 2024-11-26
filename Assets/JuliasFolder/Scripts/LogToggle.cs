using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace

public class LogToggle : MonoBehaviour
{
    // Reference to the log panel (the Scroll View containing the logs)
    public GameObject logPanel;
    
    // Reference to the button so we can update the text
    public Button toggleButton;
    
    // Reference to the TextMeshPro component on the button (to change its text)
    public TMP_Text buttonText;

    // State to track if the panel is visible
    private bool isLogPanelVisible = false;

    public void Start()
    {
        // Ensure the button is hooked up and the initial state is correct
        toggleButton.onClick.AddListener(ToggleLogPanel);
        
        // Optionally, update the initial button label
        UpdateButtonLabel();
    }

    private void ToggleLogPanel()
    {
        // Toggle visibility of the log panel
        isLogPanelVisible = !isLogPanelVisible;
        
        // Set the active state of the log panel
        logPanel.SetActive(isLogPanelVisible);

        // Update the button text to indicate the next action
        UpdateButtonLabel();
    }

    private void UpdateButtonLabel()
    {
        // Change the button label depending on the visibility state
        if (isLogPanelVisible)
        {
            buttonText.text = "Hide Logs";  // Change to "Hide Logs" when visible
        }
        else
        {
            buttonText.text = "Show Logs";  // Change to "Show Logs" when hidden
        }
    }
}
