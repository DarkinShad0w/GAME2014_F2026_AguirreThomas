using UnityEngine;
using System;

public class BulletFactory
{
    private readonly Bullet prefab;
    private readonly Action<Bullet> release;
    private readonly Transform parent;

    public BulletFactory(Bullet prefab, Action<Bullet> release, Transform parent = null)
    {
        this.prefab = prefab;
        this.release = release;
        this.parent = parent;
    }

    public Bullet Create()
    {
        Bullet bullet = UnityEngine.Object.Instantiate(prefab, parent);
        bullet.Setup(release);
        return bullet;
    }
}
