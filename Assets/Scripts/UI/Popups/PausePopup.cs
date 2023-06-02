using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : Popup
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Toggle _soundToggle;
    
    protected override void OnOpen(ViewParam viewParam)
    {
        _soundToggle.isOn = !SoundManager.IsOn;
        
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);

        NetworkController.Disconnected += OnDisconnected;
    }

    private void OnPlayButtonClicked()
    {
        PopupManager.CloseLast();
    }

    private void OnMainMenuButtonClicked()
    {
        NetworkController.Disconnect();
    }

    private void OnDisconnected()
    {
        PopupManager.CloseLast();
        WindowManager.Open<LobbyWindow>();
        SceneHandler.Change("LobbyScene");
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
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
        NetworkController.Disconnected += OnDisconnected;
    }
}