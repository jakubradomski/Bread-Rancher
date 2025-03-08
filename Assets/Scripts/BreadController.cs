using System.Threading;
using UnityEngine;

public class BreadController : MonoBehaviour
{
    public float jumpForce = 5f;
    public float jumpForceVariation = 2f;
    public bool keepUprigth = true;
    public float uprightStrength = 10f;
    public float rotationOffset = 90f;
    public float rotationStrength = 5f;
    public float moveSpeed = 2f;
    public float moveTimeMin = 1f;
    public float moveTimeMax = 5f;
    public float standingTimeMin = 1f;
    public float standingTimeMax = 20f;
    public float jumpTimeMin = 3f;
    public float jumpTimeMax = 25f;

    private Rigidbody _rigidbody;
    private Vector3 moveDirection;
    private float timer;
    private float jumpTimer;

    private BreadState state;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
        PickNewDirection();
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        if (keepUprigth)
            KeepUpright();

        if (jumpTimer < 0)
            Jump();

        switch (state)
        {
            case BreadState.Standing:
                if (timer < 0)
                    SetState(BreadState.Moving);
                break;

            case BreadState.Moving:
                if (timer < 0)
                    SetState(BreadState.Standing);
                MoveBread();
                break;
        }
    }

    void Jump()
    {
        jumpTimer = Random.Range(jumpTimeMin, jumpTimeMax);
        _rigidbody.AddForce(
            (transform.up + transform.right) * Random.Range(jumpForce - jumpForceVariation, jumpForce + jumpForceVariation),
            ForceMode.VelocityChange);
    }

    void SetState(BreadState newState)
    {
        if (state == newState)
            return;

        state = newState;

        switch(newState)
        {
            case BreadState.Standing:
                timer = Random.Range(standingTimeMin, standingTimeMax);
                return;

            case BreadState.Moving:
                timer = Random.Range(moveTimeMin, moveTimeMax);
                PickNewDirection();
                return;
        }
    }

    void KeepUpright()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;

        _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * uprightStrength));
    }

    void MoveBread()
    {
        _rigidbody.AddForce(transform.right * moveSpeed, ForceMode.Acceleration);

        Quaternion targetRotation = Quaternion.Euler(0, rotationOffset, 0)
             * Quaternion.LookRotation(moveDirection);

        _rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationStrength));
    }

    void PickNewDirection()
    {
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    public void GrindToFibrousPowder()
    {
        BreadSpawner.Instance.BreadCount--;
        Destroy(gameObject);
    }
}

public enum BreadState
{
    Standing,
    Moving
}
