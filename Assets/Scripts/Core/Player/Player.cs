using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _interactionDistance = 2f;
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Transform _holdPosition;
    [SerializeField] private float _maxLookAngle = 60f;
    [SerializeField] private float _gravity = -9.81f;

    private Vector3 _velocity;
    private bool _isGrounded;
    private PickupInventory _inventory;
    private BaseInputService _inputService;
    private InteractionService _interactionService;
    private CharacterController _characterController;
    private PickableItem _currentlyHeldItem;
    private float _verticalRotation;

    [Inject]
    private void Construct(
        PickupInventory inventory,
        BaseInputService inputService,
        InteractionService interactionService)
    {
        _inventory = inventory;
        _inputService = inputService;
        _interactionService = interactionService;
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        InitializeHoldPosition();
    }

    private void InitializeHoldPosition()
    {
        if (_holdPosition == null)
        {
            GameObject holder = new GameObject("ItemHolder");
            _holdPosition = holder.transform;
            _holdPosition.SetParent(_playerCamera);
            _holdPosition.localPosition = new Vector3(0, -0.5f, 1f);
            _holdPosition.localRotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandleGravity();
        HandleMovement();
        HandleRotation();
        HandleInteraction();
    }

    private void CheckGrounded()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    private void HandleGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleMovement()
    {
        Vector2 input = _inputService.GetMovementInput();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        _characterController.Move(move * Time.deltaTime * _moveSpeed);
    }

    private void HandleRotation()
    {
        Vector2 lookInput = _inputService.GetLookInput();

        transform.Rotate(Vector3.up, lookInput.x * _rotationSpeed);

        _verticalRotation -= lookInput.y * _rotationSpeed;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_maxLookAngle, _maxLookAngle);
        _playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    private void HandleInteraction()
    {
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        if (_inputService.IsTouchOnJoysticks(touch.position)) return;

        ProcessTouchInteraction(touch);
    }

    private void ProcessTouchInteraction(Touch touch)
    {
        Ray ray = _playerCamera.GetComponent<Camera>().ScreenPointToRay(touch.position);
        if (!Physics.Raycast(ray, out RaycastHit hit, _interactionDistance)) return;

        if (_currentlyHeldItem != null)
        {
            if (hit.transform == _currentlyHeldItem.Transform)
            {
                DropItem();
            }
            return;
        }

        var pickable = hit.collider.GetComponent<PickableItem>();
        if (pickable != null && pickable.IsPickable)
        {
            float distance = Vector3.Distance(transform.position, hit.point);
            if (distance <= _interactionDistance)
            {
                TryPickUpItem(pickable);
            }
        }
    }

    private void TryPickUpItem(PickableItem pickable)
    {
        _currentlyHeldItem = pickable;
        pickable.OnPickedUp();

        pickable.Transform.SetParent(_holdPosition);
        pickable.Transform.localPosition = Vector3.zero;
        pickable.Transform.localRotation = Quaternion.identity;
    }

    private void DropItem()
    {
        if (_currentlyHeldItem == null) return;

        _currentlyHeldItem.Transform.SetParent(null);

        var rb = _currentlyHeldItem.Transform.GetComponent<Rigidbody>();
        Vector3 dropPosition = _holdPosition.position + _playerCamera.forward * 1f;
        _currentlyHeldItem.Transform.position = dropPosition;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        _currentlyHeldItem.OnDropped();
        _currentlyHeldItem = null;
    }

    public bool CanPickUpItems => _currentlyHeldItem == null;

    public void PickUpItem(PickableItem item)
    {
        if (CanPickUpItems)
        {
            _currentlyHeldItem = item;
            item.OnPickedUp();
        }
    }
}