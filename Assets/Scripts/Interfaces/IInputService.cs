using UnityEngine;

public interface IInputService
{
    Vector2 GetMovementInput();
    Vector2 GetLookInput();
    bool IsTouchOnJoysticks(Vector2 position);
    // ¬озвращает raycastHit дл€ проверки, во что попало касание
    bool TryGetTouchInteraction(out RaycastHit hit);
}