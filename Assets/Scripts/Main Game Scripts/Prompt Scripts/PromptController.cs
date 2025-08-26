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
        model.ShowPrompt(promptMessage, secondsToClosePrompt, actionToProceed);
    }
}
