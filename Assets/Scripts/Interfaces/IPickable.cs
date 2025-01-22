using UnityEngine;

public interface IPickable
{
    void OnPickedUp();
    bool IsPickable { get; }
    Transform Transform { get; }
}
