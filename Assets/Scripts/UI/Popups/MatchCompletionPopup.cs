using UnityEngine;
using UnityEngine.UI;

public class MatchCompletionPopup : Popup
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _rewardText;
    
    protected override void OnOpen(ViewParam viewParam)
    {
        NetworkController.Disconnect();
        
        if (viewParam is not Param param)
        {
            Debug.LogError("MatchCompletionPopup: Not Found ViewParam");
            return;
        }
        
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

        _resultText.text = param.IsWin ? "VICTORY" : "DEFEAT";
        _rewardText.text = param.Reward.ToString();
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        WindowManager.Open<LobbyWindow>();
    }

    protected override void OnClose()
    {
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
    }

    public class Param : ViewParam
    {
        public readonly bool IsWin;
        public readonly int Reward;

        public Param(bool isWin, int reward)
        {
            IsWin = isWin;
            Reward = reward;
        }
    }
}