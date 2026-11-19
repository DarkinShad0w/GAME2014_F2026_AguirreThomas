using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private bool respawnOnFall = true;
    [SerializeField] private float fallLimitY = -10f;
    [SerializeField] private float gizmoWidth = 200f;
    [SerializeField] private Color fallLineColor = Color.red;

    private Rigidbody2D rb;
    private Vector2 respawnPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
    }

    void Update()
    {
        if (respawnOnFall && transform.position.y < fallLimitY)
            Respawn();
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.position = respawnPosition;
        transform.position = respawnPosition;
    }

    public void SetCheckpoint(Vector2 position)
    {
        respawnPosition = position;
    }

    private void OnDrawGizmos()
    {
        if (!respawnOnFall)
            return;

        float x = transform.position.x;
        Vector3 left = new Vector3(x - gizmoWidth / 2f, fallLimitY, 0f);
        Vector3 right = new Vector3(x + gizmoWidth / 2f, fallLimitY, 0f);

        Gizmos.color = fallLineColor;
        Gizmos.DrawLine(left, right);

        UnityEditor.Handles.color = fallLineColor;
        UnityEditor.Handles.Label(new Vector3(x, fallLimitY + 0.5f, 0f), "Fall Limit");

        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(respawnPosition, 0.5f);
        }
    }
}
