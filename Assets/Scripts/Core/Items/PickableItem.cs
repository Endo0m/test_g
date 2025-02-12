using UnityEngine;

public class PickableItem : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isPickedUp;
    private Vector3 _originalScale;

    public bool IsPickable => !_isPickedUp;
    public Transform Transform => transform;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originalScale = transform.localScale;
    }

    public void OnPickedUp()
    {
        _isPickedUp = true;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
    }

    public void OnDropped()
    {
        _isPickedUp = false;
        transform.localScale = _originalScale;
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
        }
    }

    public void Interact(Player player)
    {
        if (player.CanPickUpItems)
        {
            player.PickUpItem(this);
        }
    }
}