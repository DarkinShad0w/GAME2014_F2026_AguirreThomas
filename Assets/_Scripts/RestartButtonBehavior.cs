using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonBehavior : MonoBehaviour
{
    public void OnRestartButtonPressed()
    {
        Debug.Log("Restart Button Pressed");
        SceneManager.LoadScene("Play");
    }
}
