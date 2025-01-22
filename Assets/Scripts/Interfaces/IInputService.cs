using UnityEngine;

public interface IInputService
{
    Vector2 GetMovementInput();
    Vector2 GetLookInput();
    bool IsTouchOnJoysticks(Vector2 position);
    // ���������� raycastHit ��� ��������, �� ��� ������ �������
    bool TryGetTouchInteraction(out RaycastHit hit);
}