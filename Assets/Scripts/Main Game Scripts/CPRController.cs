using UnityEngine;

public class CPRController : MonoBehaviour
{
    public static CPRController Instance;
    public CPRView view;
    public CPRModel model;
    private void Awake()
    {
        Instance = this;
    }
    public void StartSimulation()
    {
        ClickableActionModel clickableActionModel = ClickableActionController.Instance.model;
        model.StartSimulation();
        clickableActionModel.EnableCPRProcess();
    }
}
