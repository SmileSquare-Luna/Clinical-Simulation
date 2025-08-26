using System.Collections.Generic;

[System.Serializable]
public class InstructionData
{
    public List<string> Instructions = new List<string>(); 
}
[System.Serializable]
public class AskActionData
{
    public int AskIndex;
    public string AskActionQuestion;
    public string AskActionOptionA;
    public string AskActionOptionB;

    public AskActionData(int askIndex, string askActionQuestion, string askActionOptionA, string askActionOptionB)
    {
        AskIndex = askIndex;
        AskActionQuestion = askActionQuestion;
        AskActionOptionA = askActionOptionA;
        AskActionOptionB = askActionOptionB;
    }
} 