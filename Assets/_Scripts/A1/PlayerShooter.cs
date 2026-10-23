using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private PlayerHealth playerHealth;

    [Header("Shooting")]
    [SerializeField, Min(0.05f)] private float cooldown = 0.25f;
    [SerializeField, Min(0.1f)] private float bulletSpeed = 12f;
    [SerializeField] private float despawnY = 7f;

    private ObjectPool<Bullet> pool;
    private float nextShotTime;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        pool = new ObjectPool<Bullet>(
            createFunc: CreateBullet,
            actionOnGet: b => b.gameObject.SetActive(true),
            actionOnRelease: b => b.gameObject.SetActive(false),
            actionOnDestroy: b => Destroy(b.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 60);
    }

    private void Start()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("PlayerShooter: assign the bullet prefab in the Inspector.", this);
            enabled = false;
        }
    }

    public void Shoot()
    {
        if (!enabled)
            return;
        if (playerHealth != null && playerHealth.IsDead)
            return;
        if (Time.time < nextShotTime)
            return;

        nextShotTime = Time.time + cooldown;

        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        origin.z = 0f;

        Bullet b = pool.Get();
        b.Launch(origin, bulletSpeed, despawnY);
    }

    private Bullet CreateBullet()
    {
        Bullet b = Instantiate(bulletPrefab);
        b.Setup(bullet => pool.Release(bullet));
        return b;
    }

    private void OnDrawGizmos()
    {
        float halfWidth = 4f;
        float centerX = transform.position.x;
        float cameraTop = float.NaN;

        Camera cam = Camera.main;
        if (cam != null)
        {
            halfWidth = cam.orthographicSize * cam.aspect + 0.5f;
            centerX = cam.transform.position.x;
            cameraTop = cam.transform.position.y + cam.orthographicSize;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(centerX - halfWidth, despawnY, 0f),
                        new Vector3(centerX + halfWidth, despawnY, 0f));

        if (!float.IsNaN(cameraTop))
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(new Vector3(centerX - halfWidth, cameraTop, 0f),
                            new Vector3(centerX + halfWidth, cameraTop, 0f));
        }
    }
}
