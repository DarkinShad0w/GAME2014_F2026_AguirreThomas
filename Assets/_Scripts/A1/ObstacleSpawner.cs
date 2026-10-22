using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle obstaclePrefab;
    [SerializeField] private Transform[] lanePoints;

    [Header("Positions")]
    [SerializeField] private float spawnY = 7f;
    [SerializeField] private float despawnY = -7f;

    [Header("Rows")]
    [SerializeField, Min(0.5f)] private float rowSpacing = 5f;
    [SerializeField, Min(1)] private int minPerRow = 1;
    [SerializeField, Min(1)] private int maxPerRow = 2;

    private ObjectPool<Obstacle> pool;
    private readonly List<int> laneBuffer = new List<int>();
    private float distanceSinceRow;

    private void Awake()
    {
        pool = new ObjectPool<Obstacle>(
            createFunc: CreateObstacle,
            actionOnGet: o => o.gameObject.SetActive(true),
            actionOnRelease: o => o.gameObject.SetActive(false),
            actionOnDestroy: o => Destroy(o.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 40);
    }

    void Start()
    {
        if (obstaclePrefab == null || lanePoints == null || lanePoints.Length < 2)
        {
            Debug.LogError("ObstacleSpawner: assign the obstacle prefab and at least 2 lane points.", this);
            enabled = false;
            return;
        }

        distanceSinceRow = rowSpacing;
    }

    void Update()
    {
        distanceSinceRow += WorldSpeed.Current * Time.deltaTime;

        while (distanceSinceRow >= rowSpacing)
        {
            distanceSinceRow -= rowSpacing;
            SpawnRow(spawnY - distanceSinceRow);
        }
    }

    private void SpawnRow(float y)
    {
        int laneCount = lanePoints.Length;
        int maxAllowed = Mathf.Min(maxPerRow, laneCount - 1);
        int minAllowed = Mathf.Min(minPerRow, maxAllowed);
        int count = Random.Range(minAllowed, maxAllowed + 1);

        laneBuffer.Clear();

        for (int i = 0; i < laneCount; i++)
            laneBuffer.Add(i);

        for (int i = 0; i < count; i++)
        {
            int j = Random.Range(i, laneCount);
            int lane = laneBuffer[j];
            laneBuffer[j] = laneBuffer[i];
            laneBuffer[i] = lane;

            Obstacle o = pool.Get();
            o.Launch(new Vector3(lanePoints[lane].position.x, y, 0f), despawnY);
        }
    }

    private Obstacle CreateObstacle()
    {
        Obstacle o = Instantiate(obstaclePrefab, transform);
        o.Setup(obstacle => pool.Release(obstacle));
        return o;
    }

    private void OnDrawGizmosSelected()
    {
        float left = -3f;
        float right = 3f;

        if (lanePoints != null && lanePoints.Length > 0 && lanePoints[0] != null)
        {
            left = right = lanePoints[0].position.x;
            foreach (Transform t in lanePoints)
            {
                if (t == null) continue;
                left = Mathf.Min(left, t.position.x);
                right = Mathf.Max(right, t.position.x);
            }
            left -= 1.5f;
            right += 1.5f;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(left, spawnY, 0f), new Vector3(right, spawnY, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(left, despawnY, 0f), new Vector3(right, despawnY, 0f));
    }
}
