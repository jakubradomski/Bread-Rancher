using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float mouseSensitivity = 0.01f;

    public float spreadRadius = 5f;

    public GameObject spawnerPrefab;

    private CharacterMovement _movement;
    private ObjectPickup _objectPickup;

    private float _horizontal;
    private float _vertical;

    private float _cameraX;
    private float _cameraY;

    private float _mouseX;
    private float _mouseY;

    private InputAction movementAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private InputAction crouchAction;
    private InputAction sprintAction;
    private InputAction spreadAction;

    private void Start()
    {        
        _movement = GetComponent<CharacterMovement>();
        _objectPickup = GetComponent<ObjectPickup>();

        _cameraX = playerCamera.transform.rotation.eulerAngles.y;
        _cameraY = playerCamera.transform.rotation.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;

        movementAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");
        crouchAction = InputSystem.actions.FindAction("Crouch");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        spreadAction = InputSystem.actions.FindAction("Spread");

        spreadAction.performed += DoSpread;
        jumpAction.performed += DoJump;
        interactAction.performed += DoInteract;
    }

    private void DoSpread(InputAction.CallbackContext obj)
    {
        if(CoinManager.Instance.Coins > 0)
        {
            CoinManager.Instance.Coins--;
            SpreadWheat(8);
        }
    }

    private void DoInteract(InputAction.CallbackContext obj)
    {
        _objectPickup.PickUp();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        _movement.Jump();
    }

    private void Update()
    {                    
        GetMovementInputs();
        UpdateCamera();
        UpdatePlayerMovementState();

        _movement.Move(new Vector2(_horizontal, _vertical));
    }


    private void GetMovementInputs()
    {
        Vector2 mov = movementAction.ReadValue<Vector2>();

        _horizontal = mov.x;
        _vertical = mov.y;

        Vector2 md = lookAction.ReadValue<Vector2>();

        _mouseX = md.x * mouseSensitivity;
        _mouseY = md.y * mouseSensitivity;
    }


    private void UpdatePlayerMovementState()
    {
        if (crouchAction.IsPressed())
        {
            _movement.ChangeState(PlayerState.crouching);
        }
        else if (sprintAction.IsPressed())
        {
            _movement.ChangeState(PlayerState.sprinting);
        }
        else
        {
            _movement.ChangeState(PlayerState.walking);
        }
    }


    private void UpdateCamera()
    {     
        _cameraX += _mouseX;
        _cameraY -= _mouseY;

        _cameraY = Mathf.Clamp(_cameraY, -89.0f, 89.0f);
        if (_cameraX > 360)
            _cameraX -= 360;
        if (_cameraX < 0)
            _cameraX += 360;

        playerCamera.transform.localRotation = Quaternion.Euler(_cameraY, 0, 0);
        transform.rotation = Quaternion.Euler(0, _cameraX, 0);
    }

    private void SpreadWheat(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Random.onUnitSphere * spreadRadius;
            pos = new Vector3(pos.x, 3, pos.z);
            pos += transform.position;

            if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 40, LayerMask.GetMask("Ground")))
            {
                var spawner = Instantiate(spawnerPrefab, hit.point, Quaternion.identity);
                BreadSpawner.Instance.Spawners.Add(spawner);
            }
        }
    }
}
