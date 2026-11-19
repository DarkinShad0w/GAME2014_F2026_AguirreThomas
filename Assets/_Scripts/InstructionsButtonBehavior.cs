using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsButtonBehavior : MonoBehaviour
{
    public void OnInstructionsButtonPressed()
    {
        //MenuSoundManager.Instance.PlayClickAndLoad("Instructions");
        SceneManager.LoadScene("Instructions");
    }
}
