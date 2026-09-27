using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButtonBehavior : MonoBehaviour
{
    public void OnNextButtonPressed()
    {
        Debug.Log("Next Button Pressed");
        SceneManager.LoadScene("End");
    }
}
