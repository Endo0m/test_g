using System.Collections.Generic;
using UnityEngine;

public class PickupInventory : MonoBehaviour
{
    private readonly int _maxItems = 1;
    private readonly List<PickableItem> _items = new List<PickableItem>();

    public bool HasSpace => _items.Count < _maxItems;

    public bool AddItem(PickableItem item)
    {
        if (!HasSpace) return false;

        _items.Add(item);
        return true;
    }
}