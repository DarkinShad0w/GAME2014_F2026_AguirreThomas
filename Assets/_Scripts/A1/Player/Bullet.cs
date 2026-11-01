using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Action<Bullet> release;
    private float speed;
    private float despawnX;
    private bool released = true;

    public void Setup(Action<Bullet> releaseCallback)
    {
        release = releaseCallback;
    }

    public void Launch(Vector3 origin, float bulletSpeed, float despawnAtX)
    {
        transform.position = origin;
        speed = bulletSpeed;
        despawnX = despawnAtX;
        released = false;
    }

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= despawnX)
            Despawn();
    }

    public void Despawn()
    {
        if (released)
            return;

        released = true;
        release?.Invoke(this);
    }
}
