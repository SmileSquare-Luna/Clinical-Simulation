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
        view.QuitButton.onClick.AddListener(QuitGame);
        currentActionIndex = 0; 
    }
    public void ActionMenuToggle(bool isOpen, GameObject menuGO)
    {
        if(menuGO != null) menuGO.SetActive(isOpen);
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
        TextMeshProUGUI optionAText = view.OptionAButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI optionBText = view.OptionBButton.GetComponentInChildren<TextMeshProUGUI>();
        view = ClickableActionController.Instance.view;
        view.SelectableActionText.text = askActionMessage;
        if (optionAText != null) optionAText.text = optionA;
        if (optionBText != null) optionBText.text = optionB; 
    }
    private void SetNextActionIndex(bool isCorrect)
    {
        PromptController promptControl = PromptController.Instance;
        CPRController cprControl = CPRController.Instance;
        AnimationModel animModel = AnimationController.Instance.model;
        ScoreRateModel s_RateModel = ScoreRateController.Instance.s_RateModel;

        switch (currentActionIndex)
        {
            case 0:
                if (isCorrect)
                {
                    currentActionIndex = 1;
                    animModel.currentAnimationIndex = 1;
                    s_RateModel.currentScore+=10;

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
                    s_RateModel.currentScore += 10;
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
                    s_RateModel.currentScore += 10;
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
                    s_RateModel.currentScore += 10;
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
                    s_RateModel.currentScore += 10;
                    promptControl.DisplayPrompt("CPR will now begin...", 3f, cprControl.StartSimulation);
                    cprControl.view.InformationMenu.SetActive(true);
                }
                else
                {
                    EndAnimation();
                }
                break;
        }
    }
    public void EndAnimation()
    {
        animController.model.CutsceneGO.SetActive(true);
        animController.model.SetCPR(false);
        CPRController.Instance.view.InformationMenu.SetActive(false);
        AnimationController.Instance.model.currentAnimationIndex = 6;
        animController.PlayAnimation(); // Will play animation here   
    }
    public void EnableCPRProcess()
    { 
        AnimationModel animModel = AnimationController.Instance.model;
        animModel.CutsceneGO.SetActive(false);
        animModel.SetCPR(true);
    }
    public void RefreshScene()
    {
        view.RetryButton.onClick.RemoveAllListeners();
        view.QuitButton.onClick.RemoveAllListeners();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }  
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
