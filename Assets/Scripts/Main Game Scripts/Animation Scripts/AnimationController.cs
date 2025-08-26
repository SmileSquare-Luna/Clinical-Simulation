using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance;
    public AnimationView view;
    public AnimationModel model;
    private void Awake()
    {
        Instance = this;
    } 
    public void PlayAnimation()
    {
        model.isPlayingAnimation = true;
        Debug.Log("Playing Animation");
    }
}
