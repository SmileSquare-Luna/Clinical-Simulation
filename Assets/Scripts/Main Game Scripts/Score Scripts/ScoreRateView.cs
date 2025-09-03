using TMPro;
using UnityEngine;

public class ScoreRateView : MonoBehaviour
{
    public GameObject RescueAssessmentMenu;
    public TextMeshProUGUI Rate_ScoreText;
    public TextMeshProUGUI Rate_FeedbackText;
     
    public void DisplayScore(int score, float duration)
    {
        RescueAssessmentMenu.SetActive(true);
        ScoreRateController.Instance.s_RateModel.StartCounting(score, duration);
    }
}
