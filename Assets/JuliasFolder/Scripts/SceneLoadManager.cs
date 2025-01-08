using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class SceneLoadManager: MonoBehaviour
{
    public void LoadFoodGame(){
        // Ensure ARSession is not destroyed between scenes
        DontDestroyOnLoad(FindObjectOfType<ARSession>());

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