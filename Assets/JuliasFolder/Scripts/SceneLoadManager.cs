using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
public class SceneLoadManager: MonoBehaviour
{
    public void LoadFoodGame(){
        
        //Ensure ARSession is not destroyed between scenes
        DontDestroyOnLoad(FindObjectOfType<ARSession>());

        SceneManager.LoadScene("FoodGame");
    }

    public void LoadInstructionsFoodGame(){
        SceneManager.LoadScene("Game3Instructions");
    }
    
    
    
}