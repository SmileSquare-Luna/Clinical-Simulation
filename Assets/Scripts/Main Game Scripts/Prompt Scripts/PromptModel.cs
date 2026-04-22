using System;
using System.Collections;
using UnityEngine;

public class PromptModel : MonoBehaviour
{
    PromptView view;

    public bool isCurrentlyOnPrompt;
     
    Action toProcess;
    bool canClickNow = false;

    private void Awake()
    {
        canClickNow = false;
    }
    private void Start()
    {
        canClickNow = false;
        view = PromptController.Instance.view; 
    }

    private void Update()
    {
        if (view.PromptPanel.activeSelf && isCurrentlyOnPrompt && toProcess != null && canClickNow)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canClickNow = false;
                view.PromptPanel.SetActive(false);
                isCurrentlyOnPrompt = false;
                toProcess();
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                canClickNow = false;
                view.PromptPanel.SetActive(false);
                isCurrentlyOnPrompt = false;
                toProcess();
            }

        }
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
        toProcess = actionToProceed;

        yield return new WaitForSeconds(secondsToClosePrompt);
        canClickNow = true;
        /*view.PromptPanel.SetActive(false);
        isCurrentlyOnPrompt = false;
        if (actionToProceed != null) actionToProceed();*/
    }
}
