using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private FloatingJoystick moveJoystick;
    [SerializeField] private FloatingJoystick lookJoystick;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PickupInventory pickupInventory;

    public override void InstallBindings()
    {
        Container.Bind<IInputService>()
            .To<MobileInputService>()
            .AsSingle()
            .WithArguments(moveJoystick, lookJoystick, mainCamera);

        Container.Bind<Camera>()
            .FromInstance(mainCamera)
            .AsSingle();

        Container.Bind<IInventory>()
            .To<PickupInventory>()
            .FromInstance(pickupInventory)
            .AsSingle();

        Container.Bind<IInteractionService>()
            .To<InteractionService>()
            .AsSingle();
    }
}