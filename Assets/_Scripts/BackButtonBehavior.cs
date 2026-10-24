using UnityEngine;

public class BackButtonBehavior : MonoBehaviour
{
    public void OnBackButtonPressed()
    {
        MenuSoundManager.Instance.PlayClickAndLoad("Start");
    }

    public void OnBackButtonPressedGame()
    {
        GameSoundManager.Instance.PlayClickAndLoad("Start");
    }
}
