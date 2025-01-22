public interface IInteractable
{
    void Interact(IPlayer player);
    bool CanInteract { get; }
}