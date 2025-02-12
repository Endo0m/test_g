using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private FloatingJoystick _moveJoystick;
    [SerializeField] private FloatingJoystick _lookJoystick;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PickupInventory _pickupInventory;

    public override void InstallBindings()
    {
        Container.Bind<BaseInputService>()
            .To<MobileInputService>()
            .AsSingle()
            .WithArguments(_moveJoystick, _lookJoystick, _mainCamera);

        Container.Bind<Camera>()
            .FromInstance(_mainCamera)
            .AsSingle();

        Container.Bind<PickupInventory>()
            .FromInstance(_pickupInventory)
            .AsSingle();

        Container.Bind<InteractionService>()
            .AsSingle();
    }
}