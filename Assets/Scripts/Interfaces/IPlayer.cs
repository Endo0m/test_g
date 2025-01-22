public interface IPlayer
{
    void PickUpItem(IPickable item);
    bool CanPickUpItems { get; }
}
