using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private Camera _gameCamera;
    
    private static CameraManager _instance;

    public static Camera UiCamera => _instance._uiCamera;
    public static Camera GameCamera => _instance._gameCamera;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
}