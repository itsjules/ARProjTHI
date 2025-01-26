// Created by JulP
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class StepManager : MonoBehaviour
{
    public static StepManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public enum StepType
    {
        PrintKiosk,
        ValidationKiosk,
        MoneyKiosk,
        finalQR
    }


    [SerializeField]
    private List<Step> steps;

    public StepType currentStep = StepType.PrintKiosk;

    public ParticleEffectSpawner particleEffectSpawner;

    

    public Step GetCurrentStep()
    {
        return steps.Find(s => s.stepType == currentStep);
    }
    
    public void LoadStep(StepType stepType)
    {
        currentStep = stepType;
        var step = steps.Find(s => s.stepType == stepType);
        if (step != null)
        {
            UIController.Instance.ShowInstruction(step.instructionImage);
            UIController.Instance.UpdateStepIndicatorPanel(stepType);
            ImageTrackingHandler.Instance.ResetProcessingState();
        }
    }

    public void OnImageTracked(string trackingImageName)
    {
        var step = steps.Find(s => s.stepType == currentStep);
        if (step != null && step.trackingImageName == trackingImageName)
        {
            UIController.Instance.HideInstruction();
            StartCoroutine(CompleteStepAfterDelay(5f));
        }
    }


    private IEnumerator CompleteStepAfterDelay(float delay)
    {
        //Show Button directly after capturing finalQR Code
        if (currentStep == StepType.finalQR)
        {
            UIController.Instance.ShowNextStepButton(() =>
            {
                ARSession arSession = FindObjectOfType<ARSession>();
                Debug.Log($"found AR Sessions {arSession}");
                if (arSession != null)
                {
                    arSession.Reset();  // Stop the current AR session
                    Debug.Log("reset ARSession");
                }
                SceneManager.LoadScene("Game3Instructions", LoadSceneMode.Single);
                
            });
            
            yield break;
        }
        //the Rest of manual steps have a delay of the Button
        yield return new WaitForSeconds(delay);
        UIController.Instance.ShowNextStepButton(() =>
        {
            if (currentStep == StepType.PrintKiosk)
            {
                LoadStep(StepType.ValidationKiosk);
            }
            else if (currentStep == StepType.ValidationKiosk)
            {
                LoadStep(StepType.MoneyKiosk);
            }
            else if (currentStep == StepType.MoneyKiosk)
            {
                LoadStep(StepType.finalQR);
                particleEffectSpawner.SpawnEffect("Confetti");
            }
            else
            {
                Debug.LogError("No action possible");
            }
        });
    }

    private void Start()
    {
        LoadStep(currentStep);
    }
}

[System.Serializable]
public class Step
{
    public StepManager.StepType stepType;
    public string trackingImageName;
    public Sprite instructionImage;
}
