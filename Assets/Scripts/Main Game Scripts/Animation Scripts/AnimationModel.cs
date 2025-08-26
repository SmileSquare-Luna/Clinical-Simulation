using System.Collections.Generic;
using UnityEngine;

public class AnimationModel : MonoBehaviour
{
    public List<float> animationTimePerAction = new List<float>();
    [HideInInspector] public int currentAnimationIndex = 0;
    public bool isPlayingAnimation = false;
    private float currentAnimationTime = 0; 
    private void Start()
    { 

        currentAnimationIndex = 0;
        AnimationController.Instance.PlayAnimation();
    }
    private void Update()
    {
        ClickableActionController actionController = ClickableActionController.Instance;
        if (!isPlayingAnimation && currentAnimationTime < 0)
        { 
            currentAnimationTime = animationTimePerAction[currentAnimationIndex];
        }

        if (isPlayingAnimation)
        { 
            currentAnimationTime -= 1 * Time.deltaTime;

            if(currentAnimationTime < 0)
            {
                isPlayingAnimation = false;
                //instructionControl.model.ToggleMenu(true, instructionControl.view.InstructionPanel); // open instruction  
                actionController.ShowActionAskMenu();
                return;
            } 
        }
    } 
}
