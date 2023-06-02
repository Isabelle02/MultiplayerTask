using UnityEngine;

public class SplashWindow : Window
{
    [SerializeField] private FadeGroup _fadeGroup;
    [SerializeField] private float _fadingAnimDuration = 0.2f;

    protected override void OnOpen(ViewParam viewParam)
    {
        SceneHandler.SceneLoaded += OnLobbySceneLoaded;
        _fadeGroup.SetAlpha(0f);
        _fadeGroup.DoFade(1f, _fadingAnimDuration).GetAwaiter().OnCompleted(() => SceneHandler.Load(SceneHandler.LobbyScene));
    }

    private void OnLobbySceneLoaded(string sceneName)
    {
        if (sceneName == SceneHandler.LobbyScene)
        {
            SoundManager.Play(Sound.Main, true);
            WindowManager.Open<LobbyWindow>();
        }
    }

    protected override void OnClose()
    {
        SceneHandler.SceneLoaded -= OnLobbySceneLoaded;
    }
}