using UnityEngine;
using UnityEngine.UI;

public class GameWindow : Window
{
    [SerializeField] private Button _pauseButton;
    
    public override void OnOpen(ViewParam viewParam)
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
    }

    private void OnPauseButtonClick()
    {
        PopupManager.Open<PausePopup>();
    }

    public override void OnClose()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClick);
    }
}