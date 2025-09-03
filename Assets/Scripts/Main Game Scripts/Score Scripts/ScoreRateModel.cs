using System.Collections;
using UnityEngine;

public class ScoreRateModel : MonoBehaviour
{
    public int currentScore = 0;
    public int currentCPRScore = 40;
    public void StartCounting(int targetValue, float duration)
    {
        StopAllCoroutines(); // In case a previous animation is running
        StartCoroutine(CountTo(targetValue, duration));
    }
    private IEnumerator CountTo(int targetValue, float duration)
    {
        ScoreRateView s_RV = ScoreRateController.Instance.s_RateView;

        int startValue = 0;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, progress));

            if (s_RV.Rate_ScoreText != null)
                s_RV.Rate_ScoreText.text = currentValue.ToString() + "%";

            s_RV.Rate_FeedbackText.text = "";
            yield return null; // wait for next frame
        }

        // Ensure final value is correct
        if (s_RV.Rate_ScoreText != null)
        {
            s_RV.Rate_ScoreText.text = targetValue.ToString() + "%";
            s_RV.Rate_FeedbackText.text = Feedback(targetValue);
        }
    }
    private string Feedback(int score)
    {
        if (score >= 95) return "Excellent Rescuer!";
        if (score >= 70 && score < 95) return "Good, but room for improvement";
        if (score < 70) return "Needs Training";

        return "";
    }
}
