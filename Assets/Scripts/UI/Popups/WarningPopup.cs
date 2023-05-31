using UnityEngine;
using UnityEngine.UI;

public class WarningPopup : Popup
{
    [SerializeField] private Text _warningText;
    [SerializeField] private Button _confirmButton;
    
    protected override void OnOpen(ViewParam viewParam)
    {
        if (viewParam is not Param param)
        {
            Debug.LogError("WarningPopup: Not Found ViewParam");
            return;
        }

        _warningText.text = param.WarningInfo;
        _confirmButton.onClick.AddListener(OnConfirmButtonCLick);
    }

    private void OnConfirmButtonCLick()
    {
        PopupManager.CloseLast();
    }

    protected override void OnClose()
    {
        _confirmButton.onClick.RemoveListener(OnConfirmButtonCLick);
    }

    public class Param : ViewParam
    {
        public string WarningInfo { get; }

        public Param(string warningInfo)
        {
            WarningInfo = warningInfo;
        }
    }
}