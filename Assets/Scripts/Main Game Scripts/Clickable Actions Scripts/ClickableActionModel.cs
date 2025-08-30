using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickableActionModel : MonoBehaviour
{
    AnimationController animController;
    [SerializeField] private ClickableActionView view;
    public List<AskActionData> AskActionList = new List<AskActionData>();
    [HideInInspector] public string selectedAction;
    [HideInInspector] public int currentActionIndex = 0; 

    private void Start()
    {
        view.RetryButton.onClick.AddListener(RefreshScene);
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
        PromptController promptControl = PromptController.Instance;
        CPRController cprControl = CPRController.Instance;
        AnimationModel animModel = AnimationController.Instance.model;
        switch (currentActionIndex)
        {
            case 0:
                if (isCorrect)
                {
                    currentActionIndex = 1;
                    animModel.currentAnimationIndex = 1;

                }
                else
                {
                    currentActionIndex = 2;
                    animModel.currentAnimationIndex = 2;
                }
                animController.PlayAnimation(); // Will play animation here 
                break;
            case 1: 
            case 2:
                if (isCorrect)
                {
                    currentActionIndex = 3;
                    animModel.currentAnimationIndex = 3;
                }
                else
                {
                    currentActionIndex = 4;
                    animModel.currentAnimationIndex = 4;
                }
                animController.PlayAnimation(); // Will play animation here 
                break;
            case 3:
                if (isCorrect)
                {
                    currentActionIndex = 4;
                    animModel.currentAnimationIndex = 4;
                }
                else
                {
                    currentActionIndex = 5;
                    animModel.currentAnimationIndex = 5;
                }
                animController.PlayAnimation(); // Will play animation here 
                break;
            case 4:
                if (isCorrect)
                {
                    currentActionIndex = 5;
                    animModel.currentAnimationIndex = 5;
                    animController.PlayAnimation(); // Will play animation here 
                }
                else
                { 
                    promptControl.DisplayPrompt("CPR will now begin...", 3f, cprControl.StartSimulation);
                    cprControl.view.InformationMenu.SetActive(true);
                } 
                break;
            case 5: 
                if (isCorrect)
                { 
                    promptControl.DisplayPrompt("CPR will now begin...", 3f, cprControl.StartSimulation);
                    cprControl.view.InformationMenu.SetActive(true);
                }
                else
                {
                    Debug.Log("Waiting for 811 to arrive.");
                    // play animation for 811 ambulance here
                    promptControl.model.ShowPrompt("Waiting for help to arrive...", 10f, RefreshScene); 
                }
                break;
        }
    }
    public void EnableCPRProcess()
    { 
        AnimationModel animModel = AnimationController.Instance.model;
        animModel.CutsceneGO.SetActive(false);
        animModel.SetCPR(true);
    }
    private void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    } 
}
