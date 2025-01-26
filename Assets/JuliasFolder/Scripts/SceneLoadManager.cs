//Created by JulP
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class SceneLoadManager: MonoBehaviour
{
    public void LoadFoodGame(){
        
     

        // Find the active AR session in the scene
        ARSession arSession = FindObjectOfType<ARSession>();
        Debug.Log($"found AR Sessions {arSession}");
        if (arSession != null)
        {
            arSession.Reset();  // Stop the current AR session
            Debug.Log("reset ARSession");
        }

        SceneManager.LoadScene("FoodGame", LoadSceneMode.Single);
    }

    
    
    
    
}