// StepManager.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private StepType currentStep = StepType.PrintKiosk;

    public void LoadStep(StepType stepType)
    {
        currentStep = stepType;
        var step = steps.Find(s => s.stepType == stepType);
        if (step != null)
        {
            UIController.Instance.ShowInstruction(step.instructionImage);
        }
    }
    
    public void OnImageTracked(string imageName)
    {
        var step = steps.Find(s => s.stepType == currentStep);
        if (step != null && step.imageName == imageName)
        {
            UIController.Instance.HideInstruction();
            StartCoroutine(CompleteStepAfterDelay(5f));
        }
    }

    
    private IEnumerator CompleteStepAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIController.Instance.ShowNextStepButton(() => {
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
            }
            else
            {
                Debug.Log("No Next Step");
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
    public string imageName;
    public Sprite instructionImage;
}
