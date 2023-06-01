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

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            _isJoystickCaptured = hit.collider == _joystick;
            _previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && _isJoystickCaptured)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dist = mousePosition - _previousMousePosition;
            var newPos = transform.position + dist;

            var targetBounds = new Bounds(newPos, _joystick.bounds.size);
            if (targetBounds.IsInBounds(_joystickBounds.bounds))
                transform.SetPositionXY(newPos.x, newPos.y);

            _previousMousePosition = mousePosition;
        }
        
        if (Input.GetMouseButtonUp(0) && _isJoystickCaptured)
        {
            transform.position = _joystickStartPos;
            _isJoystickCaptured = false;
        }
    }
}