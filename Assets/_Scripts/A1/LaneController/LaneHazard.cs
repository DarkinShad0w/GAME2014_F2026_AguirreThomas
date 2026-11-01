using System;
using UnityEngine;

public class LaneHazard : MonoBehaviour
{
    private Action<LaneHazard> releaseToPool;
    private float despawnY;
    private bool inPlay;

    public void Setup(Action<LaneHazard> release)
    {
        releaseToPool = release;
    }

    public void Launch(Vector3 position, float despawnBelowY)
    {
        transform.position = position;
        despawnY = despawnBelowY;
        inPlay = true;
        OnLaunch();
    }

    protected virtual void OnLaunch() { }

    public virtual bool DamagesPlayer => true;

    protected virtual void Update()
    {
        Vector3 p = transform.position;
        p.y -= WorldSpeed.Current * Time.deltaTime;
        transform.position = p;

        if (p.y < despawnY)
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
