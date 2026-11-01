using UnityEngine;

public class StartButtonBehavior : MonoBehaviour
{
    public void OnStartButtonPressed()
    {
        MenuSoundManager.Instance.PlayClickAndLoad("PlayA1");
    }
}
