using System;
using System.Collections;
using UnityEngine;

public class PromptModel : MonoBehaviour
{
    PromptView view;

    public bool isCurrentlyOnPrompt;

    private void Start()
    {
        view = PromptController.Instance.view;
    }

    public void ShowPrompt(string message, float secondsToClosePrompt, Action actionToProceed)
    {
        StartCoroutine(DisplayPromptMessage(message, secondsToClosePrompt, actionToProceed));
    }

    private IEnumerator DisplayPromptMessage(string message, float secondsToClosePrompt, Action actionToProceed)
    {
        view = PromptController.Instance.view;
        isCurrentlyOnPrompt = true;
        view.PromptPanel.SetActive(true);
        view.PromptText.text = message;
        yield return new WaitForSeconds(secondsToClosePrompt);
        view.PromptPanel.SetActive(false);
        isCurrentlyOnPrompt = false;
        if (actionToProceed != null) actionToProceed();
    }
}
