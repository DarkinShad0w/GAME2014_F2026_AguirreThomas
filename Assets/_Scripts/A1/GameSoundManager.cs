using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSoundManager : MonoBehaviour
{
    public static GameSoundManager Instance { get; private set; }

    [Header("Listens to")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShooter playerShooter;
    [SerializeField] private CoinCollector coinCollector;

    [Header("Music")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.5f;
    [SerializeField] private AudioClip gameOverMusicClip;

    [Header("Sound effects")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField, Range(0f, 1f)] private float shootVolume = 0.6f;

    [SerializeField] private AudioClip playerHitClip;
    [SerializeField, Range(0f, 1f)] private float playerHitVolume = 1f;

    [SerializeField] private AudioClip playerDeathClip;
    [SerializeField, Range(0f, 1f)] private float playerDeathVolume = 1f;

    [SerializeField] private AudioClip enemyDeathClip;
    [SerializeField, Range(0f, 1f)] private float enemyDeathVolume = 0.8f;

    [SerializeField] private AudioClip coinClip;
    [SerializeField, Range(0f, 1f)] private float coinVolume = 0.8f;

    [SerializeField] private AudioClip clickClip;
    [SerializeField, Range(0f, 1f)] private float clickVolume = 1f;
    [SerializeField, Min(0f)] private float maxLoadDelay = 0.4f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private int lastHearts = -1;
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

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged += OnHealthChanged;
            playerHealth.Died += OnPlayerDied;
        }

        if (playerShooter != null)
            playerShooter.Fired += OnShot;

        if (coinCollector != null)
            coinCollector.CoinCollected += OnCoinCollected;

        Enemy.Killed += OnEnemyKilled;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged -= OnHealthChanged;
            playerHealth.Died -= OnPlayerDied;
        }

        if (playerShooter != null)
            playerShooter.Fired -= OnShot;

        if (coinCollector != null)
            coinCollector.CoinCollected -= OnCoinCollected;

        Enemy.Killed -= OnEnemyKilled;
    }

    void Start()
    {
        PlayMusic(musicClip);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void OnShot()
    {
        Play(shootClip, shootVolume);
    }

    private void OnHealthChanged(int current, int max)
    {
        bool tookHit = lastHearts >= 0 && current < lastHearts;
        lastHearts = current;

        if (tookHit && current > 0)
            Play(playerHitClip, playerHitVolume);
    }

    private void OnPlayerDied()
    {
        musicSource.Stop();
        Play(playerDeathClip, playerDeathVolume);

        if (gameOverMusicClip != null)
            PlayMusic(gameOverMusicClip);
    }

    private void OnEnemyKilled(int points)
    {
        Play(enemyDeathClip, enemyDeathVolume);
    }

    private void OnCoinCollected()
    {
        Play(coinClip, coinVolume);
    }

    public void PlayClick()
    {
        Play(clickClip, clickVolume);
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

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
}
