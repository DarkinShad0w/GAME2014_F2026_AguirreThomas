using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;

    private Rigidbody2D rb;
    private int jumpsLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (jumpsLeft > 0 && TouchBegan())
        {
            ResetVerticalSpeed();
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsLeft--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpsLeft = maxJumps;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("End");
    }

    private void ResetVerticalSpeed()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }

    private bool TouchBegan()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            return true;

        return Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
    }
}
