<<<<<<< Updated upstream
=======
using System.Collections.Generic;
using TMPro;
>>>>>>> Stashed changes
using UnityEngine;

public class GameController : MonoBehaviour
{
<<<<<<< Updated upstream
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
=======
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Scrolling")]
    [SerializeField] private BackgroundScroller background;

    [Header("Spawning")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float despawnX = -15f;

    private readonly List<GameObject> pool = new List<GameObject>();
    private float timer;
    private float nextSpawn;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        Vector3 move = Vector3.left * background.speed * Time.deltaTime;

        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                continue;

            obj.transform.position += move;

            if (obj.transform.position.x <= despawnX)
                obj.SetActive(false);
        }

        timer += Time.deltaTime;

        if (timer >= nextSpawn)
        {
            timer = 0f;
            nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
            Spawn();
        }
    }

    private void Spawn()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = spawnPoint.position;
                obj.SetActive(true);
                return;
            }
        }
>>>>>>> Stashed changes
    }
}
