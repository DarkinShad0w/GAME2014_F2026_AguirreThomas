using System;
using UnityEngine;

public class Enemy : LaneHazard
{
    [SerializeField, Min(1)] private int maxHp = 2;
    [SerializeField, Min(0)] private int points = 2;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField, Min(0.01f)] private float flashDuration = 0.08f;

    public static event Action<int> Killed;
    private int hp;
    private bool dead;
    private Color normalColor;
    private float flashTimer;

    private void Awake()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            normalColor = sprite.color;
    }

    protected override void OnLaunch()
    {
        hp = maxHp;
        dead = false;
        flashTimer = 0f;

        if (sprite != null) 
            sprite.color = normalColor;
    }

    protected override void Update()
    {
        base.Update();

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0f && sprite != null)
                sprite.color = normalColor;
        }
    }

    public void TakeHit(int damage = 1)
    {
        if (dead)
            return;

        hp -= damage;

        if (hp <= 0)
        {
            dead = true;
            Killed?.Invoke(points);
            Despawn();
            return;
        }

        if (sprite != null)
        {
            sprite.color = flashColor;
            flashTimer = flashDuration;
        }
    }
}