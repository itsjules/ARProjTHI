//Created by JulP
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TMP_Text scoreText;

    //globaly accesible through Singleton pattern
    public static ScoreManager Instance;
    void Awake()
    {
        // Ensure there's only one instance of GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Don't destroy the GameManager between scenes
        }
        else
        {
            Destroy(gameObject); // If an instance already exists, destroy the new one
        }
    }

    private void Start()
    {
        score = 0;
    }

    //Update Score after Food was collected
    public void UpdateScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

}
