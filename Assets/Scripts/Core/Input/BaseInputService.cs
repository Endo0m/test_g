using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputService
{
    protected readonly Camera MainCamera;

    protected BaseInputService(Camera mainCamera)
    {
        MainCamera = mainCamera;
    }

    public abstract Vector2 GetMovementInput();
    public abstract Vector2 GetLookInput();
    public abstract bool IsTouchOnJoysticks(Vector2 touchPosition);
}