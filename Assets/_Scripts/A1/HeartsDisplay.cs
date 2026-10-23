using UnityEngine;
using UnityEngine.UI;

public class HeartsDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;

    private void OnEnable()
    {
        if (playerHealth == null)
            return;

        playerHealth.HealthChanged += Refresh;
        Refresh(playerHealth.CurrentHearts, playerHealth.MaxHearts);
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.HealthChanged -= Refresh;
    }

    private void Refresh(int current, int max)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null)
                continue;

            heartImages[i].gameObject.SetActive(i < max);
            heartImages[i].sprite = i < current ? fullSprite : emptySprite;
        }
    }
}
