using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : Popup
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Toggle _soundToggle;
    
    protected override void OnOpen(ViewParam viewParam)
    {
        SceneHandler.SceneLoaded += OnLobbySceneLoaded;
        
        _soundToggle.isOn = !SoundManager.IsOn;
        
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);
    }

    private void OnLobbySceneLoaded()
    {
        WindowManager.Open<LobbyWindow>();
    }

    private void OnPlayButtonClicked()
    {
        PopupManager.CloseLast();
    }

    private void OnMainMenuButtonClicked()
    {
        PopupManager.CloseLast();
        SceneHandler.Load("LobbyScene");
    }

    private async void OnSoundCheckBoxValueChanged(bool value)
    {
        SoundManager.IsOn = true;
        SoundManager.PlayOneShot(Sound.Toggle);
        await Task.Delay((int) (SoundManager.GetClipLength(Sound.Toggle) * 1000));
        SoundManager.IsOn = !value;
    }

    protected override void OnClose()
    {
        SceneHandler.SceneLoaded -= OnLobbySceneLoaded;
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
    }
}