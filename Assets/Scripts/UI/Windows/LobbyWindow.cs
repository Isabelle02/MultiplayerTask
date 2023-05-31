using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyWindow : Window
{
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private InputField _newLobbyName;
    [SerializeField] private InputField _existingLobbyName;
    [SerializeField] private Button _createLobby;
    [SerializeField] private Button _enterLobby;

    public override void OnOpen(ViewParam viewParam)
    {
        SceneHandler.SceneLoaded += OnGameSceneLoaded;
        
        _soundToggle.isOn = !SoundManager.IsOn;
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);
        
        _createLobby.onClick.AddListener(OnCreateLobbyClick);
        _enterLobby.onClick.AddListener(OnEnterLobbyClick);
    }

    private void OnGameSceneLoaded()
    {
        WindowManager.Open<GameWindow>();
    }

    private void OnEnterLobbyClick()
    {
        if (NetworkController.JoinLobby(_existingLobbyName.text))
        {
            SceneHandler.Load("GameScene");
        }
        else
        {
            NetworkController.TryToConnect();
        }
    }

    private void OnCreateLobbyClick()
    {
        if (NetworkController.JoinLobby(_newLobbyName.text))
        {
            SceneHandler.Load("GameScene");
        }
        else
        {
            NetworkController.TryToConnect();
        }
    }

    private async void OnSoundCheckBoxValueChanged(bool value)
    {
        SoundManager.IsOn = true;
        SoundManager.PlayOneShot(Sound.Toggle);
        await Task.Delay((int) (SoundManager.GetClipLength(Sound.Toggle) * 1000));
        SoundManager.IsOn = !value;
    }

    public override void OnClose()
    {
        SceneHandler.SceneLoaded -= OnGameSceneLoaded;
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
        _createLobby.onClick.RemoveListener(OnCreateLobbyClick);
        _enterLobby.onClick.RemoveListener(OnEnterLobbyClick);
    }
}