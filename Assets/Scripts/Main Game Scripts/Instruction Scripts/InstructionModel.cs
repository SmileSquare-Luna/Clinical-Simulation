using System;
using UnityEngine;

public class InstructionModel : MonoBehaviour
{
    private InstructionView i_View;
    public InstructionData MyInstructions;
    [HideInInspector] public int instructionIndex = 0;
    private void Start()
    {
        instructionIndex = 0;
    }
    public void ToggleMenu(bool isOpen, GameObject menuGO)
    { 
        menuGO.SetActive(isOpen);
        HandleInstructionText(MyInstructions.Instructions[instructionIndex]); 
    }
    public void HandleInstructionText(string instructionMessage)
    {
        i_View = InstructionController.Instance.view; 
        if (i_View == null) return; 
        i_View.InstructionText.text = instructionMessage; 
    }

}

