using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private JoystickController joystick;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float deadZone = 0.1f;
    [SerializeField] private bool flipSprite = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float x = joystick != null ? joystick.Horizontal : 0f;
        moveInput = Mathf.Abs(x) < deadZone ? 0f : x;

        if (flipSprite && spriteRenderer != null && moveInput != 0f)
            spriteRenderer.flipX = moveInput < 0f;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocityY);
    }
}
