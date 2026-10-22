using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Action<Obstacle> releaseToPool;
    private float despawnY;
    private bool inPlay;

    public void Setup(Action<Obstacle> release)
    {
        releaseToPool = release;
    }

    public void Launch(Vector3 position, float despawnBelowY)
    {
        transform.position = position;
        despawnY = despawnBelowY;
        inPlay = true;
    }

    void Update()
    {
        Vector3 p = transform.position;
        p.y -= WorldSpeed.Current * Time.deltaTime;
        transform.position = p;

        if (p.y < despawnY)
            Despawn();
    }

    public void Despawn()
    {
        if (!inPlay && releaseToPool != null) 
            return;

        inPlay = false;

        if (releaseToPool != null)
            releaseToPool(this);
        else
            Destroy(gameObject);
    }
}
