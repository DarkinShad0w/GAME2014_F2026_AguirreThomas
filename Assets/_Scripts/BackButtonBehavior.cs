using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButtonBehavior : MonoBehaviour
{
    public void OnBackButtonPressed()
    {
        //MenuSoundManager.Instance.PlayClickAndLoad("Start");
        SceneManager.LoadScene("Start");
    }

    public void OnBackButtonPressedGame()
    {
        GameSoundManager.Instance.PlayClickAndLoad("Start");
    }
}
