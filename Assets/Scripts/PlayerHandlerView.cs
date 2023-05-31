using UnityEngine;
using UnityEngine.UI;

public class PlayerHandlerView : MonoBehaviour
{
    [SerializeField] private Collider2D _joystick;
    [SerializeField] private Image _boundsImage;

    private Vector3 _joystickStartPos;
    private bool _isJoystickCaptured;

    public Vector3 MovingOffset => _joystick.transform.localPosition - _joystickStartPos;

    private void Awake()
    {
        _joystickStartPos = _joystick.transform.localPosition;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            _isJoystickCaptured = hit.collider == _joystick;
        }

        if (Input.GetMouseButton(0) && _isJoystickCaptured)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var bounds = _boundsImage.GetBounds();
            var joystickSize = _joystick.bounds.size;
            
            var min = new Vector2(-bounds.size.x - joystickSize.x / 2,
                -bounds.size.y - joystickSize.y / 2);
            var max = new Vector2(bounds.size.x + joystickSize.x / 2,
                bounds.size.y + joystickSize.y / 2);
            
            var targetX = Mathf.Clamp(mousePosition.x, min.x, max.x);
            var targetY = Mathf.Clamp(mousePosition.y, min.y, max.y);
            _joystick.transform.SetLocalPositionXY(targetX, targetY);
        }
        
        if (Input.GetMouseButtonUp(0) && _isJoystickCaptured)
        {
            _joystick.transform.localPosition = _joystickStartPos;
            _isJoystickCaptured = false;
        }
    }
}