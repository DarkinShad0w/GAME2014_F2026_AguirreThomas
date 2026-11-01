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

    public void Jump()
    {
        if (jumpsLeft <= 0)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsLeft--;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumpsLeft = maxJumps;
    }
}
