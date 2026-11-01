using TMPro;
using UnityEngine;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private CoinCollector coinCollector;
    [SerializeField] private TMP_Text label;
    [SerializeField] private string format = "{0}";

    private void OnEnable()
    {
        if (coinCollector == null)
            return;

        coinCollector.CoinsChanged += Refresh;
        Refresh(coinCollector.Coins);
    }

    private void OnDisable()
    {
        if (coinCollector != null)
            coinCollector.CoinsChanged -= Refresh;
    }

    private void Refresh(int coins)
    {
        if (label != null)
            label.text = string.Format(format, coins);
    }
}
