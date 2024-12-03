using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadManager: MonoBehaviour
{
    public void LoadValidationScene()
    {
        SceneManager.LoadScene("ValidationStep");
    }
    public void LoadChargingScene()
    {
        SceneManager.LoadScene("ChargingStep");
    }
    public void LoadQRHintScene()
    {
        SceneManager.LoadScene("FinalQRStep");
    }
}