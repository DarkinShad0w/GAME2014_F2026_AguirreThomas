using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private static CheckpointController current;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (current == this)
            return;

        PlayerRespawn player = other.GetComponentInParent<PlayerRespawn>();
        if (player == null)
            return;

        current = this;

        Vector2 point = spawnPoint != null ? spawnPoint.position : transform.position;
        player.SetCheckpoint(point);
    }

    private void OnDestroy()
    {
        if (current == this)
            current = null;
    }
}
