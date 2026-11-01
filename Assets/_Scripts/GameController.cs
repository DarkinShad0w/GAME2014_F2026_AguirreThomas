using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Final score (optional)")]
    [SerializeField] private CoinCollector coinCollector;
    [SerializeField] private TMP_Text finalScoreLabel;
    [SerializeField] private string finalScoreFormat = "Score: {0}";

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        WorldSpeed.Current = WorldSpeed.StartSpeed;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerHealth != null) playerHealth.Died += GameOver;
    }

    private void OnDisable()
    {
        if (playerHealth != null) playerHealth.Died -= GameOver;
    }

    private void GameOver()
    {
        WorldSpeed.Current = 0f;

        if (finalScoreLabel != null && coinCollector != null)
            finalScoreLabel.text = string.Format(finalScoreFormat, coinCollector.Coins);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (GameSoundManager.Instance != null)
            GameSoundManager.Instance.PlayClickAndLoad("PlayA1");
        else
            SceneManager.LoadScene(scene);
    }
}
