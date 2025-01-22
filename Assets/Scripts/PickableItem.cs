using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IPickable, IInteractable
{
    private Rigidbody rb;
    private bool _isPickedUp;
    private Vector3 _originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _originalScale = transform.localScale;
    }

    public bool IsPickable => !_isPickedUp;
    public Transform Transform => transform;
    public bool CanInteract => true;

    public void OnPickedUp()
    {
        _isPickedUp = true;
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public void OnDropped()
    {
        _isPickedUp = false;
        transform.localScale = _originalScale;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    public void Interact(IPlayer player)
    {
        player.PickUpItem(this);
    }
}