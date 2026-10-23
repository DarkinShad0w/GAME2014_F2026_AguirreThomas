using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;

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

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
