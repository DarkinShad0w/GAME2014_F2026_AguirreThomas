using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Min(1)] private int damage = 1;

    private Action<Bullet> releaseToPool;
    private float speed;
    private float despawnY;
    private bool inPlay;

    public void Setup(Action<Bullet> release)
    {
        releaseToPool = release;
    }

    public void Launch(Vector3 position, float bulletSpeed, float despawnAboveY)
    {
        transform.position = position;
        speed = bulletSpeed;
        despawnY = despawnAboveY;
        inPlay = true;
    }

    private void Update()
    {
        Vector3 p = transform.position;
        p.y += speed * Time.deltaTime;
        transform.position = p;

        if (p.y > despawnY)
            Despawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inPlay) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeHit(damage);
            Despawn();
            return;
        }

        if (other.CompareTag("Obstacle"))
            Despawn();
    }

    public void Despawn()
    {
        if (!inPlay)
            return;

        inPlay = false;

        if (releaseToPool != null)
            releaseToPool(this);
        else
            Destroy(gameObject);
    }
}
