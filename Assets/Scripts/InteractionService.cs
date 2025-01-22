using UnityEngine;
using Zenject;

public class InteractionService : IInteractionService
{
    private Camera _mainCamera;
    private float _interactionDistance = 2f;

    [Inject]
    public void Construct(Camera mainCamera)
    {
        _mainCamera = mainCamera;
    }

    public void CheckInteraction(IPlayer player)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract)
            {
                interactable.Interact(player);
            }
        }
    }
}