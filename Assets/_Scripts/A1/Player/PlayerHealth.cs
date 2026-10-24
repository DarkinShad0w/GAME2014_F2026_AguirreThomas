using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHearts = 3;
    [SerializeField, Min(0f)] private float invincibleDuration = 1.5f;
    [SerializeField, Min(1f)] private float blinkRate = 8f;
    [SerializeField] private SpriteRenderer sprite;

    [Header("Events")]
    public UnityEvent onDied;

    public event Action<int, int> HealthChanged;
    public event Action Died;

    private int currentHearts;
    private float invincibleTimer;
    private bool isDead;

    public int CurrentHearts => currentHearts;
    public int MaxHearts => maxHearts;
    public bool IsDead => isDead;
    public bool IsInvincible => invincibleTimer > 0f;

    private void Awake()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        currentHearts = maxHearts;
    }

    private void Start()
    {
        HealthChanged?.Invoke(currentHearts, maxHearts);
    }

    private void Update()
    {
        if (invincibleTimer <= 0f)
            return;

        invincibleTimer -= Time.deltaTime;

        if (sprite != null)
        {
            if (invincibleTimer <= 0f)
            {
                SetAlpha(1f);
            }
            else
            {
                bool visible = Mathf.FloorToInt(invincibleTimer * blinkRate * 2f) % 2 == 0;
                SetAlpha(visible ? 1f : 0.25f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead)
            return;

        LaneHazard hazard = other.GetComponent<LaneHazard>();

        if (hazard == null || !hazard.DamagesPlayer)
            return;

        hazard.Despawn();

        if (IsInvincible)
            return;

        TakeHit();
    }

    public void TakeHit()
    {
        if (isDead || IsInvincible)
            return;

        currentHearts = Mathf.Max(0, currentHearts - 1);
        HealthChanged?.Invoke(currentHearts, maxHearts);

        if (currentHearts == 0)
        {
            isDead = true;
            invincibleTimer = 0f;

            if (sprite != null)
            {
                SetAlpha(1f);
                sprite.enabled = false;
            }

            Died?.Invoke();
            onDied?.Invoke();
            return;
        }

        invincibleTimer = invincibleDuration;
    }

    private void SetAlpha(float a)
    {
        Color c = sprite.color;
        c.a = a;
        sprite.color = c;
    }
}
