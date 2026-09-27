using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonBehavior : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        Debug.Log("Start Button Pressed");
        SceneManager.LoadScene("Play");
    }
}
