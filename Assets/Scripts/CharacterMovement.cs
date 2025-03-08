using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController _body;
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float jumpForce;
    public float gravity;
    public float fastFallTres;
    public float fastFallMul;
    public float groundedSpeedChange = 10;
    public float jumpingSpeedChange = 4;

    [Header("Pushing")]
    public float pushForce = 3f;
    public float maxPushMass = 500.0f;
    public bool canPush = true;

    public PlayerState state;

    public Transform ground;
    private Vector3 _moveVector;
    private Vector2 _moveInput;

    private void Start()
    {
        _body = GetComponent<CharacterController>();
    }

    public void Jump()
    {
        if (_body.isGrounded)
        {
            _moveVector.y = jumpForce;            
        }
    }

    public void ChangeState(PlayerState state)
    {
        this.state = state;
    }

    public void Move(Vector2 speed)
    {
        float w = _body.isGrounded ? 10 : 3;

        float m = 1;

        switch (state)
        {
            case PlayerState.walking:
                m = walkSpeed;
                break;
            case PlayerState.sprinting:
                m = sprintSpeed;
                break;
            case PlayerState.crouching:
                m = crouchSpeed;
                break;
        }

        _moveInput.x = Mathf.Lerp(_moveInput.x, speed.x * m, Time.deltaTime * w);
        _moveInput.y = Mathf.Lerp(_moveInput.y, speed.y * m, Time.deltaTime * w);
    }

    private void Update()
    {
        if (!_body.isGrounded)
        {
            _moveVector.y -= gravity * Time.deltaTime * ((_moveVector.y < fastFallTres * Time.deltaTime) ? fastFallMul : 1);            
        }
               
        _moveVector = transform.TransformDirection(new Vector3(_moveInput.x, _moveVector.y, _moveInput.y));

        _body.Move(_moveVector * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!canPush)
            return;

        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic || body.mass > maxPushMass) 
        {
            return;
        }

        if(hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, -0.35f, hit.moveDirection.z);

        body.linearVelocity = pushDir * pushForce;
    }
}

public enum PlayerState
{
    walking,
    sprinting,
    crouching,
    frozen
}