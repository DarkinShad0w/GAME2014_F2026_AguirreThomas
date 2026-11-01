using System;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    public event Action<int> CoinsChanged;
    public event Action CoinCollected;

    private int coins;
    public int Coins => coins;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        Enemy.Killed += AddPoints;
    }

    private void OnDisable()
    {
        Enemy.Killed -= AddPoints;
    }

    void Start()
    {
        CoinsChanged?.Invoke(coins);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerHealth != null && playerHealth.IsDead)
            return;

        Coin coin = other.GetComponent<Coin>();

        if (coin == null)
            return;

        coin.Despawn();
        AddPoints(coin.Value);
        CoinCollected?.Invoke();
    }

    public void AddPoints(int amount)
    {
        if (amount <= 0)
            return;

        if (playerHealth != null && playerHealth.IsDead)
            return;

        coins += amount;
        CoinsChanged?.Invoke(coins);
    }
}
