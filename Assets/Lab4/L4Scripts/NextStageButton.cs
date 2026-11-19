using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    public void OnNext1ButtonPressed()
    {
        SceneManager.LoadScene("Map2");
    }
    public void OnNext2ButtonPressed()
    {
        SceneManager.LoadScene("Map3");
    }
    public void OnNext3ButtonPressed()
    {
        SceneManager.LoadScene("End");
    }
}
