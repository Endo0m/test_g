
using UnityEngine;
using Zenject;
public class Player : MonoBehaviour, IPlayer
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float maxLookAngle = 60f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 _velocity;
    private bool _isGrounded;
    private IInventory _inventory;
    private IInputService _inputService;
    private IInteractionService _interactionService;
    private CharacterController _characterController;
    private IPickable _currentlyHeldItem;
    private float _verticalRotation = 0f;

    [Inject]
    private void Construct(
        IInventory inventory,
        IInputService inputService,
        IInteractionService interactionService)
    {
        _inventory = inventory;
        _inputService = inputService;
        _interactionService = interactionService;
        _characterController = GetComponent<CharacterController>();
    }

 
    private void Start()
    {
        if (holdPosition == null)
        {
            GameObject holder = new GameObject("ItemHolder");
            holdPosition = holder.transform;
            holdPosition.SetParent(playerCamera);
            holdPosition.localPosition = new Vector3(0, -0.5f, 1f); 
            holdPosition.localRotation = Quaternion.identity;
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
        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleMovement()
    {
        Vector2 input = _inputService.GetMovementInput();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        _characterController.Move(move * Time.deltaTime * moveSpeed);
    }

    private void HandleRotation()
    {
        Vector2 lookInput = _inputService.GetLookInput();

        transform.Rotate(Vector3.up, lookInput.x * rotationSpeed);

        _verticalRotation -= lookInput.y * rotationSpeed;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
    }

    private void HandleInteraction()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (!IsOnJoystick(touch.position))
                {
                    Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, interactionDistance)) 
                    {
                        if (_currentlyHeldItem != null)
                        {
                            if (hit.transform == _currentlyHeldItem.Transform)
                            {
                                DropItem();
                                return;
                            }
                        }
                        else
                        {
                            var pickable = hit.collider.GetComponent<IPickable>();
                            if (pickable != null && pickable.IsPickable)
                            {
                                float distance = Vector3.Distance(transform.position, hit.point);
                                if (distance <= interactionDistance)
                                {
                                    TryPickUpItem(pickable);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsOnJoystick(Vector2 touchPosition)
    {
        var inputService = _inputService as MobileInputService;
        return inputService?.IsTouchOnJoysticks(touchPosition) ?? false;
    }

    private void TryPickUpItem(IPickable pickable)
    {
        _currentlyHeldItem = pickable;
        pickable.OnPickedUp();

        pickable.Transform.SetParent(holdPosition);
        pickable.Transform.localPosition = Vector3.zero;
        pickable.Transform.localRotation = Quaternion.identity;
    }

    private void DropItem()
    {
        if (_currentlyHeldItem != null)
        {
            _currentlyHeldItem.Transform.SetParent(null);

            var rb = _currentlyHeldItem.Transform.GetComponent<Rigidbody>();
            var pickableItem = _currentlyHeldItem.Transform.GetComponent<PickableItem>();

            Vector3 dropPosition = holdPosition.position + playerCamera.forward * 1f;
            _currentlyHeldItem.Transform.position = dropPosition;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            if (pickableItem != null)
            {
                pickableItem.OnDropped();
            }

            _currentlyHeldItem = null;
        }
    }
    public bool CanPickUpItems => _currentlyHeldItem == null;

    public void PickUpItem(IPickable item)
    {
        if (CanPickUpItems)
        {
            _currentlyHeldItem = item;
            item.OnPickedUp();
        }
    }
}