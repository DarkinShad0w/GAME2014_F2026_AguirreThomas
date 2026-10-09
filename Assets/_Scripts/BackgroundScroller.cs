using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scrolling")]
    [SerializeField] private float speed = 2f;

    [Header("Spawning")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int poolSize = 5;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 3f;
    [SerializeField] private float despawnX = -15f;

    private Transform copy;
    private float width;
    private float startX;

    private readonly List<GameObject> pool = new List<GameObject>();
    private float timer;
    private float nextSpawn;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        startX = transform.position.x;
        enabled = false;
        GameObject clone = Instantiate(gameObject, transform.position + Vector3.right * width,
                                       transform.rotation, transform.parent);
        enabled = true;

        Destroy(clone.GetComponent<BackgroundScroller>());
        copy = clone.transform;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        Vector3 move = Vector3.left * speed * Time.deltaTime;
        transform.position += move;
        copy.position += move;

        if (transform.position.x <= startX - width) 
            transform.position += Vector3.right * width * 2f;

        if (copy.position.x <= startX - width) 
            copy.position += Vector3.right * width * 2f;

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
    }
}
