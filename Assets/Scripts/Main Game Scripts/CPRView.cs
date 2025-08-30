using TMPro;
using UnityEngine;

public class CPRView : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI CompressCountText;
    public TextMeshProUGUI CompressIndicatorText;
    [Header("Information UI")]
    public GameObject InformationMenu;

    private void Start()
    { 
        InformationMenu.SetActive(false);
        CompressIndicatorText.text = "";
    }

}
