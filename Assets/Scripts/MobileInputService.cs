
using UnityEngine;

public class MobileInputService : IInputService
{
    private readonly FloatingJoystick _moveJoystick;
    private readonly FloatingJoystick _lookJoystick;
    private readonly Camera _mainCamera;

    public MobileInputService(FloatingJoystick moveJoystick, FloatingJoystick lookJoystick, Camera mainCamera)
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

    public bool TryGetTouchInteraction(out RaycastHit hit)
    {
        hit = new RaycastHit();

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began && !IsTouchOnJoysticks(touch.position))
            {
                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    return true;
                }
            }
        }

        return false;
    }
}