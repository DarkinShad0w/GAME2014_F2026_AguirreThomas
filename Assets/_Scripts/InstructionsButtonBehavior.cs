using UnityEngine;

public class InstructionsButtonBehavior : MonoBehaviour
{
    public void OnInstructionsButtonPressed()
    {
        MenuSoundManager.Instance.PlayClickAndLoad("Instructions");
    }
}
