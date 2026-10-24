using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSoundManager : MonoBehaviour
{
    public static MenuSoundManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;

    [Header("Button click")]
    [SerializeField] private AudioClip clickClip;
    [SerializeField, Range(0f, 1f)] private float clickVolume = 1f;
    [SerializeField, Min(0f)] private float maxLoadDelay = 0.4f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private bool loading;

    private void Awake()
    {
        Instance = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        if (musicClip == null)
            return;

        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    private void OnDestroy()
    {
        if (Instance == this) 
            Instance = null;
    }

    public void PlayClick()
    {
        if (clickClip != null)
            sfxSource.PlayOneShot(clickClip, clickVolume);
    }

    public void PlayClickAndLoad(string sceneName)
    {
        if (loading)
            return;
        loading = true;

        PlayClick();
        StartCoroutine(LoadAfterClick(sceneName));
    }

    private IEnumerator LoadAfterClick(string sceneName)
    {
        float wait = clickClip != null ? Mathf.Min(clickClip.length, maxLoadDelay) : 0f;
        yield return new WaitForSecondsRealtime(wait);

        SceneManager.LoadScene(sceneName);
    }
}
