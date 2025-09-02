using Unity.VisualScripting;
using UnityEngine;

public class CPRModel : MonoBehaviour
{
    PromptController prompt;
    CPRView cPRView;
     
    public bool isStartCPRSimulationNow = false;
    public bool canStartCompression = false; 
    public bool isReadyHeadTiltChinLiftManeuver = false;
    private float currentSeconds = 0f;
    [SerializeField] private float startingSeconds = 60f;
    private int currentCompressCount = 0; 
    float lastPressTime = 0f; 

    [SerializeField] private int countOfCPRApplied = 0;
    [SerializeField] private int maxCPRToAwakeVictim = 3;


    private void Start()
    {
        prompt = PromptController.Instance;
        cPRView = CPRController.Instance.view;

        maxCPRToAwakeVictim = Random.Range(1, 7);

        canStartCompression = false;
        isStartCPRSimulationNow = false;
        isReadyHeadTiltChinLiftManeuver = false;
        currentSeconds = startingSeconds;
        currentCompressCount = 0; 
        countOfCPRApplied = 0; 
    }
    private void Update()
    {
        if(cPRView == null) cPRView = CPRController.Instance.view;
        if(prompt == null) prompt = PromptController.Instance;
         
        StartCPRTimer();
        StartCPRCompression();
        StartHeadTiltChinLiftManeuver(); 
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

            if (currentSeconds > 1)
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
        if (isStartCPRSimulationNow && canStartCompression && !prompt.model.isCurrentlyOnPrompt)
        {
            AnimationModel animModel = AnimationController.Instance.model;
            bool isAnimationCompleted = animModel.CheckPlayerAnimationCompleted("compression");
             

            if (CompressionControl())
            {  
                animModel.ClickCompress();

                if (isAnimationCompleted)
                { 
                    currentCompressCount++;
                    cPRView.CompressCountText.text = $"{currentCompressCount}/30";

                    CompressionMeasure();
                } 

                if (currentCompressCount >= 30)
                {
                    animModel.currentlyPlayingAnimation = false;
                    isReadyHeadTiltChinLiftManeuver = true;
                    canStartCompression = false;
                }
            }
        }
    }
    private void CompressionMeasure()
    {
        if (isStartCPRSimulationNow && canStartCompression && !prompt.model.isCurrentlyOnPrompt)
        { 

            if (CompressionControl())
            {
                float now = Time.time;
                float interval = now - lastPressTime;

                if (lastPressTime > 0)
                {
                    if (interval > 0.62f)
                    {
                        cPRView.CompressIndicatorText.text = "To slow!";
                    }
                    else if (interval < 0.48f)
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
    private void StartHeadTiltChinLiftManeuver()
    {
        if (isReadyHeadTiltChinLiftManeuver)
        {

            AnimationModel animModel = AnimationController.Instance.model; 
            bool isAnimationCompleted = animModel.CheckPlayerAnimationCompleted("give breath"); 


            if (isAnimationCompleted)
            {
                isReadyHeadTiltChinLiftManeuver = false;
                // start CPR again 
                RestartCPR();
            }
            else
            { 
                animModel.GiveBreath();
            }
        }
    }
    private void RestartCPR()
    {
        lastPressTime = 0; 
        canStartCompression = true;

        currentCompressCount = 0;
        cPRView.CompressCountText.text = $"{currentCompressCount}/30";
        cPRView.CompressIndicatorText.text = "";


        prompt.DisplayPrompt("Do another set of compressions.”!", 1f, CheckFinishCPR);
    }
    private bool CompressionControl()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE 
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
        #elif UNITY_IOS || UNITY_ANDROID
        // Mobile devices
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
        #endif

            return false;
    }
    private void CheckFinishCPR()
    {
        countOfCPRApplied++;
        if (countOfCPRApplied >= maxCPRToAwakeVictim)
        {
            cPRView.InformationMenu.SetActive(false);
            ClickableActionController.Instance.model.RefreshScene();
        }
        else
        {
            cPRView.InformationMenu.SetActive(true);
        }
    }
}
