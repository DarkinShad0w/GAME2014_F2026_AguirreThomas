using UnityEngine;
using UnityEngine.Pool;
using System;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private PlayerHealth playerHealth;

    [Header("Shooting")]
    [SerializeField, Min(0.05f)] private float cooldown = 0.25f;
    [SerializeField, Min(0.1f)] private float bulletSpeed = 6f;
    [SerializeField] private float despawnX = 7f;


    public event Action Fired;
    private ObjectPool<Bullet> pool;
    private BulletFactory factory;
    private float nextShotTime;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (bulletPrefab == null)
        {
            Debug.LogError("PlayerShooter: assign the bullet prefab in the Inspector.", this);
            enabled = false;
            return;
        }

        factory = new BulletFactory(bulletPrefab, b => pool.Release(b));

        pool = new ObjectPool<Bullet>(
            createFunc: factory.Create,
            actionOnGet: b => b.gameObject.SetActive(true),
            actionOnRelease: b => b.gameObject.SetActive(false),
            actionOnDestroy: b => Destroy(b.gameObject),
            collectionCheck: false,
            defaultCapacity: 20,
            maxSize: 60);
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
        b.Launch(origin, bulletSpeed, despawnX);


        Fired?.Invoke();
    }

    private void OnDrawGizmos()
    {
        float halfHeight = 4f;
        float centerY = transform.position.y;

        Camera cam = Camera.main;
        if (cam != null)
        {
            halfHeight = cam.orthographicSize + 0.5f;
            centerY = cam.transform.position.y;
        }

        // Vertical line showing where bullets despawn on the X axis.
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(despawnX, centerY - halfHeight, 0f),
                        new Vector3(despawnX, centerY + halfHeight, 0f));
    }
}
