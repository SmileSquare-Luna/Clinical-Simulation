using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimationModel : MonoBehaviour
{
   // public List<float> animationTimePerAction = new List<float>();
    [HideInInspector] public int currentAnimationIndex = 0;
    //public bool isPlayingAnimation = false;
    //private float currentAnimationTime = 0;

    [Header("Player")]
    public GameObject playerGameGO;
    private Animator playerAnim; 
    [Header("Victim")]
    public GameObject victimGameGO;
    private Animator victimAnim;

    [Header("Timeline")]
    public List<PlayableDirector> cutscenesTimeline = new List<PlayableDirector>();
    public PlayableDirector currentCutscene;
    public bool isToCheckTimeline;
   /* private void OnEnable()
    {
        if (currentCutscene != null)
        {
            currentCutscene.stopped += OnTimelineStopped;
        }
    }

    private void OnDisable()
    {
        if (currentCutscene != null)
        {
            currentCutscene.stopped -= OnTimelineStopped;
        }
    }*/

    private void Start()
    {
        playerAnim = playerGameGO.GetComponent<Animator>();
        victimAnim = victimGameGO.GetComponent<Animator>();

        currentAnimationIndex = 0;
        AnimationController.Instance.PlayAnimation();


    } 

    private void OnTimelineStopped(PlayableDirector obj)
    {
        ClickableActionController actionController = ClickableActionController.Instance;

        isToCheckTimeline = false; 
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
                isToCheckTimeline = true;


                if (currentCutscene != null)
                {
                    currentCutscene.stopped -= OnTimelineStopped;
                     
                    currentCutscene = cutscene;
                    currentCutscene.stopped += OnTimelineStopped;
                }
            }
        }
    }
}
