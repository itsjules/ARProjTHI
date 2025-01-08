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

    public void ShowInstruction(Sprite image)
    {
        instructionImage.sprite = image;
        instructionImage.gameObject.SetActive(true);
        nextStepButton.SetActive(false);
    }

    public void HideInstruction()
    {   
        Debug.Log("set InstructionImage false");
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
}