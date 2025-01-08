using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneLoader : MonoBehaviour
{
    public void LoadEndScene(){
        SceneManager.LoadScene("EndScene");
    }
}
