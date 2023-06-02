using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyWindow : Window
{
    [SerializeField] private Toggle _soundToggle;
    [SerializeField] private InputField _newRoomName;
    [SerializeField] private InputField _existingRoomName;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _enterRoomButton;

    protected override void OnOpen(ViewParam viewParam)
    {
        SceneHandler.SceneLoaded += OnGameSceneLoaded;
        
        _soundToggle.isOn = !SoundManager.IsOn;
        _soundToggle.onValueChanged.AddListener(OnSoundCheckBoxValueChanged);
        
        _createRoomButton.onClick.AddListener(OnCreateRoomClick);
        _enterRoomButton.onClick.AddListener(OnEnterRoomClick);
    }

    private void OnGameSceneLoaded(string sceneName)
    {
        if (sceneName == "GameScene")
            WindowManager.Open<GameWindow>();
    }

    private void OnEnterRoomClick()
    {
        if (NetworkController.JoinRoom(_existingRoomName.text))
            SceneHandler.Load(SceneHandler.GameScene);
        else
            NetworkController.TryToConnect();
    }

    private void OnCreateRoomClick()
    {
        if (NetworkController.CreateRoom(_newRoomName.text))
            SceneHandler.Load(SceneHandler.GameScene);
        else
            NetworkController.TryToConnect();
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
        SceneHandler.SceneLoaded -= OnGameSceneLoaded;
        _soundToggle.onValueChanged.RemoveListener(OnSoundCheckBoxValueChanged);
        _createRoomButton.onClick.RemoveListener(OnCreateRoomClick);
        _enterRoomButton.onClick.RemoveListener(OnEnterRoomClick);
    }
}