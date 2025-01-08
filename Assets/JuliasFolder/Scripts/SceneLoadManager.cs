using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadManager: MonoBehaviour
{
    public void LoadFoodGame(){
        SceneManager.LoadScene("FoodGame");
    }
    //---Depreciated from seperate Scene approach
    // public void LoadValidationScene()
    // {
    //     SceneManager.LoadScene("ValidationStep");
    // }
    // public void LoadChargingScene()
    // {
    //     SceneManager.LoadScene("ChargingStep");
    // }
    // public void LoadQRHintScene()
    // {
    //     SceneManager.LoadScene("FinalQRStep");
    // }
    
    
}