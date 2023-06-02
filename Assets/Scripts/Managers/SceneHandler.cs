using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneHandler
{
    public static string LoadingScene => "LoadingScene";
    public static string LobbyScene => "LobbyScene";
    public static string GameScene => "GameScene";
    
    public static event Action<string> SceneLoaded;
    
    public static async void Load(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName);
        SceneLoaded?.Invoke(sceneName);
    }

    public static void Change(string name)
    {
        SceneManager.LoadScene(name);
    }
}