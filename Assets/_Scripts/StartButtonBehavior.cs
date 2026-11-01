using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonBehavior : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        //MenuSoundManager.Instance.PlayClickAndLoad("PlayA1");
        SceneManager.LoadScene("Play");
    }
}
