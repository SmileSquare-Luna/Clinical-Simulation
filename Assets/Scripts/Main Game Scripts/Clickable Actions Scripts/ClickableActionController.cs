using TMPro;
using UnityEngine;

public class ClickableActionController : MonoBehaviour
{
    public static ClickableActionController Instance;
    public ClickableActionView view;
    public ClickableActionModel model;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        view.OptionAButton.onClick.AddListener(
            () => model.SelectAcion(view.OptionAButton.GetComponentInChildren<TextMeshProUGUI>().text, true));
        view.OptionBButton.onClick.AddListener(
            () => model.SelectAcion(view.OptionBButton.GetComponentInChildren<TextMeshProUGUI>().text, false));
    }
    public void ShowActionAskMenu()
    {
        model.ActionMenuToggle(true, view.SelectableActionGO);
    }
}
