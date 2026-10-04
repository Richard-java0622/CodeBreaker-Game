using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected PlayerInput playerInput;
    protected Vector2 moveInput;

    protected Vector2 lastMoveInput;

    protected bool movementEnabled = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        moveInput = Vector2.zero;
        lastMoveInput = Vector2.down;
    }

    protected void FixedUpdate()
    {
        PlayerMove();
    }

    public Vector2 GetLastMoveInput()
    {
        return lastMoveInput; // will prob used this idk 
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!movementEnabled)
        {
            return;
        }

        moveInput = context.ReadValue<Vector2>();
        moveInput.Normalize();

        if (moveInput != Vector2.zero)
        {
            lastMoveInput = moveInput;
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;

        if (!enabled)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected void PlayerMove()
    {
        if (!movementEnabled)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;

        animator.SetFloat("xVal", lastMoveInput.x);
        animator.SetFloat("yVal", lastMoveInput.y);
        animator.SetBool("IsMoving", moveInput != Vector2.zero);
    }
}
