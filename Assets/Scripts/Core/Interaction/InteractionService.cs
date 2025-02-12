using UnityEngine;
using Zenject;

public class InteractionService
{
    private readonly Camera _mainCamera;
    private readonly float _interactionDistance = 2f;

    [Inject]
    public InteractionService(Camera mainCamera)
    {
        _mainCamera = mainCamera;
    }

    public void CheckInteraction(Player player)
    {
        if (Input.touchCount <= 0) return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        if (!Physics.Raycast(ray, out RaycastHit hit, _interactionDistance)) return;

        var pickable = hit.collider.GetComponent<PickableItem>();
        if (pickable != null)
        {
            pickable.Interact(player);
        }
    }
}