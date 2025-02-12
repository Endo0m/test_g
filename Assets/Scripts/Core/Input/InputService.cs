using UnityEngine;

public class InputService
{
    private readonly FloatingJoystick _moveJoystick;
    private readonly FloatingJoystick _lookJoystick;
    private readonly Camera _mainCamera;

    public InputService(FloatingJoystick moveJoystick, FloatingJoystick lookJoystick, Camera mainCamera)
    {
        _moveJoystick = moveJoystick;
        _lookJoystick = lookJoystick;
        _mainCamera = mainCamera;
    }

    public Vector2 GetMovementInput()
    {
        return new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical);
    }

    public Vector2 GetLookInput()
    {
        return new Vector2(_lookJoystick.Horizontal, _lookJoystick.Vertical);
    }

    public bool IsTouchOnJoysticks(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_moveJoystick.GetComponent<RectTransform>(), touchPosition) ||
               RectTransformUtility.RectangleContainsScreenPoint(_lookJoystick.GetComponent<RectTransform>(), touchPosition);
    }
}