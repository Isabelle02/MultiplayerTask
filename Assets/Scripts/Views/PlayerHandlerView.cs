using UnityEngine;

public class PlayerHandlerView : MonoBehaviour
{
    [SerializeField] private Collider2D _joystick;
    [SerializeField] private CircleCollider2D _joystickBounds;

    private Vector3 _joystickStartPos;
    private bool _isJoystickCaptured;
    private Vector3 _previousMousePosition;

    public Vector3 MovingDistance => (transform.position - _joystickStartPos) / 50;
    public Vector2 Size => _joystick.bounds.size;

    private void Awake()
    {
        _joystickStartPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _previousMousePosition = CameraManager.GameCamera.ScreenToWorldPoint(Input.mousePosition);
            var ray = CameraManager.GameCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue);
            if (hit) 
                _isJoystickCaptured = hit.collider == _joystick;
        }

        if (Input.GetMouseButton(0) && _isJoystickCaptured)
        {
            var mousePosition = CameraManager.GameCamera.ScreenToWorldPoint(Input.mousePosition);
            var dist = mousePosition - _previousMousePosition;
            var newPos = transform.position + dist;
            
            var targetBounds = new Bounds(newPos, _joystick.bounds.size);
            if (targetBounds.IsInBounds(_joystickBounds.bounds))
                transform.SetPositionXY(newPos.x, newPos.y);

            _previousMousePosition = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isJoystickCaptured = false;
            transform.position = _joystickStartPos;
        }
    }

    // private void OnMouseDown()
    // {
    //     _isJoystickCaptured = true;
    //     _previousMousePosition = CameraManager.UiCamera.ScreenToWorldPoint(Input.mousePosition);
    // }
    //
    // private void OnMouseDrag()
    // {
    //     if (!_isJoystickCaptured)
    //         return;
    //     
    //     var mousePosition = CameraManager.UiCamera.ScreenToWorldPoint(Input.mousePosition);
    //     var dist = mousePosition - _previousMousePosition;
    //     var newPos = transform.position + dist;
    //
    //     var targetBounds = new Bounds(newPos, _joystick.bounds.size);
    //     if (targetBounds.IsInBounds(_joystickBounds.bounds))
    //         transform.SetPositionXY(newPos.x, newPos.y);
    //
    //     _previousMousePosition = mousePosition;
    // }
    //
    // private void OnMouseUp()
    // {
    //     _isJoystickCaptured = false;
    //     transform.position = _joystickStartPos;
    // }
}