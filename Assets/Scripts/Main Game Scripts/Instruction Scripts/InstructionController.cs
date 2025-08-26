using UnityEngine;

public class InstructionController : MonoBehaviour
{
    public static InstructionController Instance;
    public InstructionView view;
    public InstructionModel model;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //model.ToggleMenu(true, view.InstructionPanel); // showing instruction
        view.InstructionOkayBTN.onClick.AddListener(CloseInstruction);
    }
    private void CloseInstruction()
    {
        //ClickableActionController actionController = ClickableActionController.Instance;

        model.ToggleMenu(false, view.InstructionPanel);
        //actionController.ShowActionAskMenu();
    }

    
}
