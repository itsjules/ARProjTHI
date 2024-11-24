using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum StepID{
        INITIAL_DETECTION,
        PRINTING_INSTRUCTIONS,
        PRINTED_CARD_INSTRUCTIONS,
        VALIDATION_INSTRUCTIONS,
        VALIDATED_CARD_INSTRUCTIONS,
        MONEY_KIOSK_INSTRUCTIONS, 
        FINAL_INSTRUCTIONS
    }
public class StepManager : MonoBehaviour
{
    public static StepManager Instance { get; private set; }

    private void Awake()
    {
        transform.parent = null;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    [Tooltip("The current Step in the AR Manual")]
    [SerializeField] public StepID currentStep;
    
    //For events
    public Action<int> OnStepChanged = delegate { };


    private void Start() {
       currentStep=StepID.INITIAL_DETECTION;
    }

    public void NextStep(){
        
    }




  #region module class Step
    public class Step{
        StepID stepID;
        string trackableText="";
        Vector3 trackableOffset;
        string canvasText="";
        bool buttonState;
        string requiredImageName;
        bool isComplete;
    }
   #endregion
    
}