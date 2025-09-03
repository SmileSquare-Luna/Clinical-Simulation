using System;
using UnityEngine;

public class PromptController : MonoBehaviour
{
    public static PromptController Instance;
    public PromptView view;
    public PromptModel model;
    private void Awake()
    {
        Instance = this;
    }

    public void DisplayPrompt(string promptMessage, float secondsToClosePrompt, Action actionToProceed)
    {
        CPRModel cPR_Model = CPRController.Instance.model;
        CPRView cPR_View = CPRController.Instance.view;
        if (promptMessage == "Do another set of compressions.") cPR_Model.countOfCPRApplied++;

        if (cPR_Model.countOfCPRApplied >= cPR_Model.maxCPRToAwakeVictim)
        {
            cPR_View.InformationMenu.SetActive(false);
            ClickableActionController.Instance.model.EndAnimation();
        }
        else
        {
            cPR_View.InformationMenu.SetActive(true);
            model.ShowPrompt(promptMessage, secondsToClosePrompt, actionToProceed);
        }
    }
}
