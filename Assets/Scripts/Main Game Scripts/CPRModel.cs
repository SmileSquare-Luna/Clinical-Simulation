using Unity.VisualScripting;
using UnityEngine;

public class CPRModel : MonoBehaviour
{
    PromptController prompt;
    CPRView cPRView;


    public bool isStartCPRSimulationNow = false;
    public bool canStartCompression = false;
    public bool isReadyToBreath = false;
    public bool isReadyHeadTiltChinLiftManeuver = false;
    private float currentSeconds = 0f;
    [SerializeField] private float startingSeconds = 60f;
    private int currentCompressCount = 0;
    private int currentBreathGiven = 0;
    float lastPressTime = 0f;

    // To remove later if the animation is now available
    private float TiltHeadChinLiftCurrentSeconds = 3f; // it's 2 seconds
    private float BreathCurrentSeconds = 2f;// it's 1 second

    [SerializeField] private int countOfCPRApplied = 0;


    private void Start()
    {
        prompt = PromptController.Instance;
        cPRView = CPRController.Instance.view;
        isReadyToBreath = false;
        canStartCompression = false;
        isStartCPRSimulationNow = false;
        isReadyHeadTiltChinLiftManeuver = false;
        currentSeconds = startingSeconds;
        currentCompressCount = 0;


        countOfCPRApplied = 0;
        TiltHeadChinLiftCurrentSeconds = 3f;
        BreathCurrentSeconds = 2f;
    }
    private void Update()
    {
        if(cPRView == null) cPRView = CPRController.Instance.view;
        if(prompt == null) prompt = PromptController.Instance;

        StartCPRTimer();
        StartCPRCompression();
        StartHeadTiltChinLiftManeuver();
        ApplyBreath();
        CompressionMeasure();
    }
    public void StartSimulation()
    {
        if(!isStartCPRSimulationNow) isStartCPRSimulationNow = true;
        canStartCompression = true;
    }
    private void StartCPRTimer()
    { 
        if (isStartCPRSimulationNow)
        {
            currentSeconds -= 1f * Time.deltaTime;

            if (currentSeconds > 0)
            {
                cPRView.TimerText.text = currentSeconds > 1 ? $"{(int)currentSeconds} seconds" : $"{(int)currentSeconds} second";
            }
            else
            { 
                currentSeconds = startingSeconds;

                cPRView.TimerText.text = currentSeconds > 1 ? $"{(int)currentSeconds} seconds" : $"{(int)currentSeconds} second";
            }
        }
    }
    private void StartCPRCompression()
    {
        if (isStartCPRSimulationNow && canStartCompression && !isReadyToBreath)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentCompressCount++;
                cPRView.CompressCountText.text = $"{currentCompressCount}/30";

                if (currentCompressCount >= 30)
                {
                    isReadyHeadTiltChinLiftManeuver = true;
                    canStartCompression = false;
                }
            }
        }
    }
    private void CompressionMeasure()
    {
        if (isStartCPRSimulationNow && canStartCompression && !isReadyToBreath)
        {
            if (Input.GetMouseButtonDown(0))
            {
                float now = Time.time;
                float interval = now - lastPressTime;

                if (lastPressTime > 0)
                {
                    if (interval > 0.65f)
                    {
                        cPRView.CompressIndicatorText.text = "To slow!";
                    }
                    else if (interval < 0.45f)
                    {
                        cPRView.CompressIndicatorText.text = "Too fast!";
                    }
                    else
                    {
                        cPRView.CompressIndicatorText.text = "Good compression speed!";
                    }
                }

                lastPressTime = now;
            }
        }
    }
    private void ApplyBreath()
    {
        if (isReadyToBreath && !isReadyHeadTiltChinLiftManeuver)
        {
            if(currentBreathGiven < 2)
            {
                BreathCurrentSeconds -= 1 * Time.deltaTime;
                if (BreathCurrentSeconds > 0)
                {
                    // play animation here which will breath on the victim
                    Debug.Log("Playing animation to breath on the victim for 1 second");
                }
                else
                {
                    currentBreathGiven++;
                    BreathCurrentSeconds = 2f;
                }
            }
            else
            {
                // start CPR again 
                RestartCPR();
            }
        }
    }
    private void StartHeadTiltChinLiftManeuver()
    {
        if (isReadyHeadTiltChinLiftManeuver)
        {
            TiltHeadChinLiftCurrentSeconds -= 1f;

            if(TiltHeadChinLiftCurrentSeconds > 0)  
            {
                // play animation here which will tilt the head and lift chin in 2 seconds
                Debug.Log("Playing animation for tilt head and lift chin");
            }
            else
            {
                isReadyHeadTiltChinLiftManeuver = false;
                isReadyToBreath = true; 
                TiltHeadChinLiftCurrentSeconds = 3f;
                BreathCurrentSeconds = 2f;
            }
        }
    }
    private void RestartCPR()
    {
        isReadyToBreath = false;
        canStartCompression = true;
        countOfCPRApplied++;
        currentCompressCount = 0;
        currentBreathGiven = 0;
        cPRView.CompressCountText.text = $"{currentCompressCount}/30";
        prompt.DisplayPrompt("Apply Compression again!", 1f, null) ;
    }

}
