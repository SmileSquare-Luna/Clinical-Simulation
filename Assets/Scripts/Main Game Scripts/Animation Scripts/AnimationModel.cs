using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationModel : MonoBehaviour
{ 
    [HideInInspector] public int currentAnimationIndex = 0; 

    [Header("CPR Process")]
    public GameObject CPRCamera;
    public GameObject playerGameGO;
    private Animator playerAnim;  
    public GameObject victimGameGO;
    private Animator victimAnim;

    private bool hasStarted = false;
    private bool hasCompleted = false;
    public bool currentlyPlayingAnimation;

    [Space(20)]
    [Header("Cutscene")]
    public List<PlayableDirector> cutscenesTimeline = new List<PlayableDirector>();
    public PlayableDirector currentCutscene;
    public GameObject CutsceneGO;
    private void Start()
    {
        playerAnim = playerGameGO.GetComponent<Animator>();
        victimAnim = victimGameGO.GetComponent<Animator>();

        currentAnimationIndex = 0;
        AnimationController.Instance.PlayAnimation(); 
        SetCPR(false);

    } 

    private void OnTimelineStopped(PlayableDirector obj)
    {
        ClickableActionController actionController = ClickableActionController.Instance;
         
        actionController.ShowActionAskMenu();
        Debug.Log("Done Cutscene!");
    }
    public void PlayCutscene(PlayableDirector obj)
    {
        if (obj == null) return;
        EnableCutscene(obj); 
    }

    private void EnableCutscene(PlayableDirector obj)
    { 
        foreach (PlayableDirector cutscene in cutscenesTimeline)
        {
            if (obj != cutscene)
            {
                cutscene.gameObject.SetActive(false);
            }
            else if (obj == cutscene)
            {
                cutscene.gameObject.SetActive(true);
                cutscene.Play();  
                if (currentCutscene != null)
                {
                    currentCutscene.stopped -= OnTimelineStopped;
                     
                    currentCutscene = cutscene;
                    currentCutscene.stopped += OnTimelineStopped;
                }
            }
        }
    }
    public void SetCPR(bool isVisible)
    {
        playerGameGO.SetActive(isVisible);
        victimGameGO.SetActive(isVisible);
        CPRCamera.SetActive(isVisible);
    }
    public void ClickCompress()
    {

        if (!currentlyPlayingAnimation)
        {
            currentlyPlayingAnimation = true;
            playerAnim.SetBool("isCompress", true);
            playerAnim.SetBool("isDoneCPR", false);
            playerAnim.SetBool("isGiveBreath", false);
            playerAnim.Play("compression", 0, 0f);
        }
    }
    public void DoneCPR()
    {
        if (!currentlyPlayingAnimation)
        {
            currentlyPlayingAnimation = true;
            playerAnim.SetBool("isCompress", false);
            playerAnim.SetBool("isDoneCPR", true);
            playerAnim.SetBool("isGiveBreath", false);
            playerAnim.Play("stand", 0, 0f);
        }
    }
    public void GiveBreath()
    {
        if (!currentlyPlayingAnimation)
        {
            currentlyPlayingAnimation = true;
            playerAnim.SetBool("isCompress", false);
            playerAnim.SetBool("isDoneCPR", false);
            playerAnim.SetBool("isGiveBreath", true);
            playerAnim.Play("give breath", 0, 0f);
        }
    }
    public void StopAnimation()
    {
        playerAnim.SetBool("isCompress", false);
        playerAnim.SetBool("isDoneCPR", false);
        playerAnim.SetBool("isGiveBreath", false);
    }
    public bool CheckPlayerAnimationCompleted(string stateName)
    {
        if (playerAnim == null) return false;

        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(stateName))
        {
            // Reset flags when animation begins
            if (!hasStarted && stateInfo.normalizedTime < 0.1f)
            {
                hasStarted = true;
                hasCompleted = false;
                Debug.Log($"{stateName} started");
            }
             

            // Detect when finished
            if (hasStarted && !hasCompleted && !stateInfo.loop && stateInfo.normalizedTime >= 1.0f)
            {
                hasCompleted = true;
                Debug.Log($"{stateName} finished!");
                if (currentlyPlayingAnimation)
                {
                    currentlyPlayingAnimation = false;
                }
                // return true; // only true once
            } 
            if (hasStarted && hasCompleted && stateInfo.normalizedTime >= 1.0f)
            {
                hasCompleted = false;
                if(currentlyPlayingAnimation)
                {
                    currentlyPlayingAnimation = false;
                }
                return true; // only true once
            }
        }
        else
        {
            // Reset when not in this state anymore
            hasStarted = false;
        }

        return false;
    }
}
