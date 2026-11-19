using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonBehavior : MonoBehaviour
{
    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("Start");
    }
}
