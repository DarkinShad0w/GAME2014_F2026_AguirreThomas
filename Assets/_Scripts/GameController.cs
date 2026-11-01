using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameController : MonoBehaviour
{
    [System.Serializable]
    private class Spawnable
    {
        public GameObject prefab;
        public int poolSize = 5;
        public float weight = 1f;
        public Transform spawnPoint;
        [System.NonSerialized] public List<GameObject> pool = new List<GameObject>();
    }

    [SerializeField] private PlayerHealth playerHealth;

    [Header("Scrolling")]
    [SerializeField] private BackgroundScroller background;

    [Header("Spawning")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Spawnable[] spawnables;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float despawnX = -15f;

    private float timer;
    private float nextSpawn;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start()
    {
        foreach(Spawnable s in spawnables)
        {
            if (s.prefab == null)
                continue;

            for (int i = 0; i < s.poolSize; i++)
            {
                GameObject obj = Instantiate(s.prefab);
                obj.SetActive(false);
                s.pool.Add(obj);
            }
        }

        nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        Vector3 move = Vector3.left * background.speed * Time.deltaTime;

        foreach (Spawnable s in spawnables)
        {
            foreach (GameObject obj in s.pool)
            {
                if (!obj.activeInHierarchy)
                    continue;

                obj.transform.position += move;

                if (obj.transform.position.x <= despawnX)
                    obj.SetActive(false);
            }
        }

        timer += Time.deltaTime;

        if (timer >= nextSpawn)
        {
            timer = 0f;
            nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
            Spawn();
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.Died += OnPlayerDied;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.Died -= OnPlayerDied;
    }

    private void Spawn()
    {
        Spawnable chosen = PickWeighted();
        if (chosen == null)
            return;

        foreach (GameObject obj in chosen.pool)
        {
            if (!obj.activeInHierarchy)
            {
                Transform point = chosen.spawnPoint != null ? chosen.spawnPoint : spawnPoint;
                obj.transform.position = point.position;
                obj.SetActive(true);
                return;
            }
        }
    }

    private Spawnable PickWeighted()
    {
        float total = 0f;

        foreach (Spawnable s in spawnables)
        {
            if (s.prefab != null)
                total += s.weight;
        }

        if (total <= 0f)
            return null;

        float roll = Random.value * total;

        foreach (Spawnable s in spawnables)
        {
            if (s.prefab == null)
                continue;

            roll -= s.weight;
            if (roll <= 0f)
                return s;
        }

        return null;
    }

    private void OnPlayerDied()
    {
        SceneManager.LoadScene("End");
    }
}
