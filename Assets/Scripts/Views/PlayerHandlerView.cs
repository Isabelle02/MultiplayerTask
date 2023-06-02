using UnityEngine;

public class PlayerHandlerView : MonoBehaviour
{
    [SerializeField] private Collider2D _joystick;
    [SerializeField] private CircleCollider2D _joystickBounds;

    private Vector3 _joystickStartPos;
    private bool _isJoystickCaptured;
    private Vector3 _previousMousePosition;

    public Vector3 MovingOffset => (transform.position - _joystickStartPos) / 100;

    private void Awake()
    {
        _joystickStartPos = transform.position;
    }

    private void OnMouseDown()
    {
        _isJoystickCaptured = true;
        _previousMousePosition = CameraManager.UiCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        var mousePosition = CameraManager.UiCamera.ScreenToWorldPoint(Input.mousePosition);
        var dist = mousePosition - _previousMousePosition;
        var newPos = transform.position + dist;

        var targetBounds = new Bounds(newPos, _joystick.bounds.size);
        if (targetBounds.IsInBounds(_joystickBounds.bounds))
            transform.SetPositionXY(newPos.x, newPos.y);

        _previousMousePosition = mousePosition;
    }

    private void OnMouseUp()
    {
        _isJoystickCaptured = false;
        transform.position = _joystickStartPos;
    }
}