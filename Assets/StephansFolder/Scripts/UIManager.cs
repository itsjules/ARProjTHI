using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject Canvas_welcome;          // Begr��ungs-Canvas
    public GameObject Canvas_Leveloverview;   // Erste Level-�bersicht
    public GameObject Canvas_Quizgame;        // Quiz-Canvas
    public GameObject Canvas_Leveloverview02; // Zweite Level-�bersicht
    public GameObject Canvas_Hint01;          // Hint-Canvas (neue Szene)

    // Methode zum Wechsel von Welcome zu Leveloverview
    public void OnWelcomeContinueClicked()
    {
        Canvas_welcome.SetActive(false);
        Canvas_Leveloverview.SetActive(true); // Wechsel zur Level�bersicht
    }

    // Methode zum Wechsel von Leveloverview zu QuizGame
    public void OnQuizStartClicked()
    {
        Canvas_Leveloverview.SetActive(false);
        Canvas_Quizgame.SetActive(true); // Wechsel zum Quiz-Canvas
    }

    // Methode zum Wechsel vom QuizGame zur zweiten Level�bersicht
    public void OnQuizEndClicked()
    {
        Canvas_Quizgame.SetActive(false);
        Canvas_Leveloverview02.SetActive(true); // Wechsel zur zweiten Level�bersicht
    }

    // Methode zum Wechsel von Leveloverview02 zu Hint01
    public void OnHintContinueClicked()
    {
        Canvas_Leveloverview02.SetActive(false);
        Canvas_Hint01.SetActive(true); // Wechsel zu Hint01-Canvas
    }
}
