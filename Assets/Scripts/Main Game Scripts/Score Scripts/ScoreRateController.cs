using UnityEngine;

public class ScoreRateController : MonoBehaviour
{
    public static ScoreRateController Instance;
    public ScoreRateView s_RateView;
    public ScoreRateModel s_RateModel;
    private void Awake()
    {
        Instance = this;
    } 
}
