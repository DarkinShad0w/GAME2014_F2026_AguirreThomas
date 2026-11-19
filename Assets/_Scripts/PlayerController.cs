using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private JoystickController joystick;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float deadZone = 0.1f;
    [SerializeField] private bool flipSprite = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 1;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private string walkingParameter = "IsWalking";
    private string jumpingParameter = "IsJumping";
    private string velocityYParameter = "VelocityY";

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private int jumpsLeft;
    private bool isGrounded;
    private int walkAnimId;
    private int jumpAnimId;
    private int velocityYId;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        walkAnimId = Animator.StringToHash(walkingParameter);
        jumpAnimId = Animator.StringToHash(jumpingParameter);
        velocityYId = Animator.StringToHash(velocityYParameter);

        jumpsLeft = maxJumps;
    }

    private void Update()
    {
        // --- Ground check ---
        isGrounded = CheckGrounded();

        if (isGrounded && rb.linearVelocityY <= 0.01f)
            jumpsLeft = maxJumps;

        // --- Horizontal input ---
        float x = joystick != null ? joystick.Horizontal : 0f;
        moveInput = Mathf.Abs(x) < deadZone ? 0f : x;

        if (flipSprite && spriteRenderer != null && moveInput != 0f)
            spriteRenderer.flipX = moveInput < 0f;

        // --- Animation ---
        if (animator != null)
        {
            animator.SetBool(walkAnimId, moveInput != 0f);
            animator.SetBool(jumpAnimId, !isGrounded);
            animator.SetFloat(velocityYId, rb.linearVelocityY);
        } 
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);
    }

    public void Jump()
    {
        if (jumpsLeft <= 0)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsLeft--;
        isGrounded = false;
    }

    private bool CheckGrounded()
    {
        if (groundCheck == null)
            return false;

        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer) != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = isGrounded ? Color.green : Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}
