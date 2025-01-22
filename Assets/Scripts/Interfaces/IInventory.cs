public interface IInventory
{
    bool AddItem(IPickable item);
    bool HasSpace { get; }
}