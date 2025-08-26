using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickableActionModel : MonoBehaviour
{
    AnimationController animController;
    ClickableActionView view;
    public List<AskActionData> AskActionList = new List<AskActionData>();
    [HideInInspector] public string selectedAction;
    [HideInInspector] public int currentActionIndex = 0; 
    private void Start()
    {
        currentActionIndex = 0; 
    }
    public void ActionMenuToggle(bool isOpen, GameObject menuGO)
    {
        menuGO.SetActive(isOpen);
        string question = AskActionList[currentActionIndex].AskActionQuestion;
        string optionA = AskActionList[currentActionIndex].AskActionOptionA;
        string optionB = AskActionList[currentActionIndex].AskActionOptionB;  

        if (isOpen)
            ShowActionMenu(question, optionA, optionB);
    }
    public void SelectAcion(string action, bool isCorrect)
    {
        view = ClickableActionController.Instance.view;
        animController = AnimationController.Instance;

        selectedAction = action;
        ActionMenuToggle(false, view.SelectableActionGO); 
        SetNextActionIndex(isCorrect);

    }
    private void ShowActionMenu(string askActionMessage, string optionA, string optionB)
    {
        view = ClickableActionController.Instance.view;
        view.SelectableActionText.text = askActionMessage;
        view.OptionAButton.GetComponentInChildren<TextMeshProUGUI>().text = optionA;
        view.OptionBButton.GetComponentInChildren<TextMeshProUGUI>().text = optionB; 
    }

    private void SetNextActionIndex(bool isCorrect)
    {
        switch (currentActionIndex)
        {
            case 0:
                if (isCorrect)
                {
                    currentActionIndex = 1; 
                }
                else
                {
                    currentActionIndex = 2;
                }
                animController.PlayAnimation(); // Will play animation here 
                break;
            case 1: 
            case 2:
                if (isCorrect)
                {
                    currentActionIndex = 3;  
                }
                else
                {
                    currentActionIndex = 4;
                }
                animController.PlayAnimation(); // Will play animation here 
                break;
            case 3:
                if (isCorrect)
                {
                    currentActionIndex = 4; 
                }
                else
                {
                    currentActionIndex = 5;
                }
                animController.PlayAnimation(); // Will play animation here 
                break;
            case 4:
                if (isCorrect)
                {
                    currentActionIndex = 5;
                    animController.PlayAnimation(); // Will play animation here 
                }
                else
                {
                    Debug.Log("Start CPR");
                } 
                break;
            case 5: 
                if (isCorrect)
                {
                    Debug.Log("Start CPR. Show Button");
                }
                else
                {
                    Debug.Log("Waiting for 811"); 
                    // play animation for 811 ambulance here
                }
                break;
        }
    }
}
