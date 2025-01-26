//Created by JulP
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    
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
    
    [SerializeField]
    private Image instructionImage;

    [SerializeField]
    private GameObject nextStepButton;

    [SerializeField]
    private GameObject ARManualUI;
    [SerializeField]
    private GameObject Canvas_leveloverview03;
    [SerializeField]
    private GameObject Canvas_InstructionsGame3;

    [Header("StepIndicatorPanel Things")]
    [SerializeField]
    private GameObject StepIndicatorPanel;

    //Different StepPane Images, I know not the cleanest way to do it by quickNdirty it is for now
    public Sprite printKioskInactive;
    public Sprite printKioskActive;
    public Sprite printKioskDone;

    public Sprite validationKioskInactive;
    public Sprite validationKioskActive;
    public Sprite validationKioskDone;

    public Sprite moneyKioskInactive;
    public Sprite moneyKioskActive;
    public Sprite moneyKioskDone;

    

    public void ShowInstruction(Sprite image)
    {
        instructionImage.sprite = image;
        instructionImage.gameObject.SetActive(true);
        nextStepButton.SetActive(false);
    }

    public void HideInstruction()
    {   
        // Debug.Log("set InstructionImage false");
        //need to trigger slide out here, otherwise it gets disabled before coroutine even start
        instructionImage.GetComponent<SlideInOutAnimation>().StartSlideOut();

        instructionImage.gameObject.SetActive(false);
    }

    public void ShowNextStepButton(UnityEngine.Events.UnityAction onClickAction)
    {
        nextStepButton.SetActive(true);
        nextStepButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextStepButton.GetComponent<Button>().onClick.AddListener(onClickAction);
    }

    //Disable Manual UI and show Level overview for Food Game
    public void ShowLevelOverview(){
        ARManualUI.SetActive(false);
        Canvas_leveloverview03.SetActive(true);
    }

    //Disable LevelOverview UI and show Instruction for Food Game
    public void ShowGame3Instruction(){
        Canvas_leveloverview03.SetActive(false);
        Canvas_InstructionsGame3.SetActive(true);
    }

    //Update StepIndicatorPanel Images (quickNdirty)
    public void UpdateStepIndicatorPanel(StepManager.StepType currentStep){
        Image PrintStepImage= StepIndicatorPanel.transform.Find("Print/Image").gameObject.GetComponent<Image>();
        Image ValidateStepImage= StepIndicatorPanel.transform.Find("Validation/Image").gameObject.GetComponent<Image>();
        Image MoneyStepImage= StepIndicatorPanel.transform.Find("Money/Image").gameObject.GetComponent<Image>();
        switch (currentStep)
        {
            case StepManager.StepType.PrintKiosk:
            PrintStepImage.sprite=printKioskActive;
            ValidateStepImage.sprite=validationKioskInactive;
            MoneyStepImage.sprite=moneyKioskInactive;
            break;

            case StepManager.StepType.ValidationKiosk:
            PrintStepImage.sprite=printKioskDone;
            ValidateStepImage.sprite=validationKioskActive;
            MoneyStepImage.sprite=moneyKioskInactive;
            break;

            case StepManager.StepType.MoneyKiosk:
            PrintStepImage.sprite=printKioskDone;
            ValidateStepImage.sprite=validationKioskDone;
            MoneyStepImage.sprite=moneyKioskActive;
            break;

            case StepManager.StepType.finalQR:
            PrintStepImage.sprite=printKioskDone;
            ValidateStepImage.sprite=validationKioskDone;
            MoneyStepImage.sprite=moneyKioskDone;
            break;
            
            default:
            Debug.Log("Update not possible");
            break;
        }

    }




}