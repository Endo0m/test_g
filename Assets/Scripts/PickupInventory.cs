using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInventory : MonoBehaviour, IInventory
{
    private int _maxItems = 1;
    private List<IPickable> _items = new List<IPickable>();

    public bool HasSpace => _items.Count < _maxItems;

    public bool AddItem(IPickable item)
    {
        if (!HasSpace) return false;

        _items.Add(item);
        return true;
    }
}