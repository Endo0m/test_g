
using UnityEngine;

public class MobileInputService : BaseInputService
{
    private readonly FloatingJoystick _moveJoystick;
    private readonly FloatingJoystick _lookJoystick;

    public MobileInputService(
        FloatingJoystick moveJoystick,
        FloatingJoystick lookJoystick,
        Camera mainCamera) : base(mainCamera)
    {
        _moveJoystick = moveJoystick;
        _lookJoystick = lookJoystick;
    }

    public override Vector2 GetMovementInput()
    {
        return new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical);
    }

    public override Vector2 GetLookInput()
    {
        return new Vector2(_lookJoystick.Horizontal, _lookJoystick.Vertical);
    }

    public override bool IsTouchOnJoysticks(Vector2 touchPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(_moveJoystick.GetComponent<RectTransform>(), touchPosition) ||
               RectTransformUtility.RectangleContainsScreenPoint(_lookJoystick.GetComponent<RectTransform>(), touchPosition);
    }
}