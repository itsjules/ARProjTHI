using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string QuestionText;
        public string[] Answers; // 4 Antworten
        public int CorrectAnswerIndex; // Index der richtigen Antwort
    }

    public UnityEngine.UI.Text QuestionText; // Verknüpfen mit QuestionText im Canvas
    public Button[] AnswerButtons; // Verknüpfen mit BtnAnswerA-D
    public UnityEngine.UI.Text[] AnswerTexts; // Verknüpfen mit AnswerTextA-D
    public Button NextQuestionButton; // Verknüpfen mit Btn_NextQuestion
    public UnityEngine.UI.Text ResultText; // Verknüpfen mit ResultText
    public Button ContinueButton; // Verknüpfen mit Btn_Continue

    // Felder für Sprites
    public Sprite CorrectAnswerSprite; // Sprite für richtige Antwort
    public Sprite WrongAnswerSprite; // Sprite für falsche Antwort
    public Sprite DefaultButtonSprite; // Standard-Button-Sprite

    public List<Question> Questions = new List<Question>(); // Liste der Fragen
    private int currentQuestionIndex = 0;
    private bool answered = false;

    private void Start()
    {
        NextQuestionButton.interactable = false; // Button deaktivieren
        NextQuestionButton.onClick.AddListener(ShowNextQuestion);

        // Continue-Button und ResultText ausblenden
        ContinueButton.gameObject.SetActive(false);
        ResultText.gameObject.SetActive(false);

        foreach (Button button in AnswerButtons)
        {
            button.onClick.AddListener(() => OnAnswerSelected(button));
        }

        ShowQuestion();
    }

    private void ShowQuestion()
    {
        answered = false;
        NextQuestionButton.interactable = false;

        if (currentQuestionIndex < Questions.Count)
        {
            // Zeige die aktuelle Frage
            Question currentQuestion = Questions[currentQuestionIndex];
            QuestionText.text = currentQuestion.QuestionText;

            for (int i = 0; i < AnswerButtons.Length; i++)
            {
                AnswerTexts[i].text = currentQuestion.Answers[i];
                AnswerButtons[i].interactable = true; // Buttons aktivieren
                AnswerButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = DefaultButtonSprite; // Sprite zurücksetzen
                AnswerButtons[i].gameObject.SetActive(true); // Buttons sichtbar machen
                AnswerButtons[i].transform.localScale = Vector3.one; // Skalierung zurücksetzen
            }

            QuestionText.gameObject.SetActive(true); // Frage-Text sichtbar machen
        }
        else
        {
            // Quiz abgeschlossen, Ergebnisse anzeigen
            ShowResults();
        }
    }

    private void OnAnswerSelected(Button selectedButton)
    {
        if (answered) return;

        answered = true;

        Question currentQuestion = Questions[currentQuestionIndex];
        int selectedIndex = System.Array.IndexOf(AnswerButtons, selectedButton);

        if (selectedIndex == currentQuestion.CorrectAnswerIndex)
        {
            selectedButton.GetComponent<UnityEngine.UI.Image>().sprite = CorrectAnswerSprite; // Richtiges Sprite setzen
            StartCoroutine(BounceButton(selectedButton.transform)); // Bounce-Effekt starten
        }
        else
        {
            selectedButton.GetComponent<UnityEngine.UI.Image>().sprite = WrongAnswerSprite; // Falsches Sprite setzen
            AnswerButtons[currentQuestion.CorrectAnswerIndex].GetComponent<UnityEngine.UI.Image>().sprite = CorrectAnswerSprite; // Richtiges Sprite hervorheben
            StartCoroutine(BounceButton(AnswerButtons[currentQuestion.CorrectAnswerIndex].transform)); // Bounce-Effekt für richtigen Button starten
        }

        foreach (Button button in AnswerButtons)
        {
            button.interactable = false; // Buttons deaktivieren
        }

        NextQuestionButton.interactable = true; // Next-Button aktivieren
    }

    private IEnumerator BounceButton(Transform buttonTransform)
    {
        // Originalgröße speichern
        Vector3 originalScale = buttonTransform.localScale;

        // Auf 102% skalieren
        Vector3 scaleUp = originalScale * 1.05f;
        // Auf 98% skalieren
        Vector3 scaleDown = originalScale * 0.98f;

        // Animation ausführen
        float duration = 0.1f; // Dauer für eine Animation
        for (int i = 0; i < 1; i++) // 2 Bounces
        {
            // Hoch skalieren
            yield return ScaleOverTime(buttonTransform, scaleUp, duration);
            // Runter skalieren
            yield return ScaleOverTime(buttonTransform, scaleDown, duration);
        }
        // Zurück zur Originalgröße
        yield return ScaleOverTime(buttonTransform, originalScale, duration);
    }

    private IEnumerator ScaleOverTime(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = target.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale; // Endgültige Skalierung setzen
    }

    private void ShowNextQuestion()
    {
        foreach (Button button in AnswerButtons)
        {
            button.GetComponent<UnityEngine.UI.Image>().sprite = DefaultButtonSprite; // Standard-Sprite wiederherstellen
        }

        currentQuestionIndex++;
        ShowQuestion();
    }

    private void ShowResults()
    {
        // Frage-Text und Buttons ausblenden
        QuestionText.gameObject.SetActive(false);
        foreach (Button button in AnswerButtons)
        {
            button.gameObject.SetActive(false);
        }
        NextQuestionButton.gameObject.SetActive(false);

        // Ergebnis-Text und Continue-Button anzeigen
        ResultText.gameObject.SetActive(true);
        ResultText.text = "Herzlichen Glückwunsch! Du hast das Quiz abgeschlossen.";
        ContinueButton.gameObject.SetActive(true);
    }

    private void OnContinue()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.OnQuizEndClicked();
        }
        else
        {
            UnityEngine.Debug.LogError("UIManager nicht gefunden!");
        }
    }
}
