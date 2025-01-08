using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class ScoreManager : MonoBehaviour
{
    public int healthyScore = 0;
    public int unhealthyScore = 0;
    public TMP_Text healthyScoreText;
    public TMP_Text unhealthyScoreText;
    public TMP_Text timerText;
    public TMP_Text resultText; // Display result after game ends
    public float gameDuration = 60f; // 60-second timer
    private float timer;
    [SerializeField]
    private  GameObject continueBttn; // Button to end the game and switch to endscene

    public static ScoreManager Instance;

    // Food categorization
    private readonly string[] healthyFoods = { "Curry plate 1", "Peach 1", "Udon 1" };
    private readonly string[] unhealthyFoods = { "Cheesecake B 1", "Hotdog A 1", "Pizza Margarita Slice 1" };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetScores();
        timer = gameDuration;
    }

    private void Update()
    {
        // Countdown timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = $"Time: {Mathf.CeilToInt(timer)}s";
        }
        else
        {
            EndGame();
        }
    }

    public void ResetScores()
    {
        healthyScore = 0;
        unhealthyScore = 0;
        resultText.text = ""; // Clear previous result text
        UpdateScoreTexts();
    }

    public void AddHealthyScore()
    {
        healthyScore++;
        UpdateScoreTexts();
    }

    public void AddUnhealthyScore()
    {
        unhealthyScore++;
        UpdateScoreTexts();
    }

    public void UpdateScoreTexts()
    {
        healthyScoreText.text = $"H: {healthyScore}";
        unhealthyScoreText.text = $"U: {unhealthyScore}";
    }

    public bool IsHealthyFood(string foodName)
    {
        foreach (string healthyFood in healthyFoods)
        {
            if (foodName.Contains(healthyFood)) return true;
        }
        return false;
    }

    public bool IsUnhealthyFood(string foodName)
    {
        foreach (string unhealthyFood in unhealthyFoods)
        {
            if (foodName.Contains(unhealthyFood)) return true;
        }
        return false;
    }

    private void EndGame()
    {
        // Stop timer
        timer = 0;

        // Determine if the user's diet was healthy or not
        bool isHealthyDiet = unhealthyScore <= healthyScore / 3;
        string resultMessage = isHealthyDiet
            ? "Super, you have a healthy diet!"
            : "Oopsie Daisy, you might want to improve your diet champ.";

        // Display result
        resultText.text = resultMessage;

        // Debug log for verification
        Debug.Log(resultMessage);

        continueBttn.SetActive(true);
    }

    
}
