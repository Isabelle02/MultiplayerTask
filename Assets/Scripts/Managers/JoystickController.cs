using UnityEngine;

public class JoystickController : MonoBehaviour
{
    [SerializeField] private PlayerHandlerView _playerHandlerView;

    public Vector3 MovingOffset => _playerHandlerView.MovingDistance;
    
    private void Awake()
    {
        var bounds = CameraManager.GameCamera.Bounds();
        var posX = bounds.min.x + 1.25f * _playerHandlerView.Size.x;
        var posY = bounds.min.y + 1.25f * _playerHandlerView.Size.y;
        transform.SetPositionXY(posX, posY);
    }
}