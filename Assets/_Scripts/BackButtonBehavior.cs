using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonBehavior : MonoBehaviour
{
    public void OnBackButtonPressed()
    {
        Debug.Log("Back Button Pressed");
        SceneManager.LoadScene("Start");
    }
}
