using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnType
    {
        public LaneHazard prefab;
        [Min(0f)] public float weight = 1f;
    }

    [SerializeField] private SpawnType[] types;
    [SerializeField] private Transform[] lanePoints;

    [Header("Positions")]
    [SerializeField] private float spawnY = 7f;
    [SerializeField] private float despawnY = -7f;

    [Header("Rows")]
    [SerializeField, Min(0.5f)] private float rowSpacing = 5f;
    [SerializeField, Min(1)] private int minPerRow = 1;
    [SerializeField, Min(1)] private int maxPerRow = 2;

    [Header("Coins (optional)")]
    [SerializeField] private Coin coinPrefab;
    [SerializeField, Range(0f, 1f)] private float coinChance = 0.5f;

    private ObjectPool<LaneHazard> coinPool;
    private ObjectPool<LaneHazard>[] pools;
    private float totalWeight;
    private readonly List<int> laneBuffer = new List<int>();
    private float distanceSinceRow;

    private void Awake()
    {
        if (types == null)
            return;

        pools = new ObjectPool<LaneHazard>[types.Length];

        for (int i = 0; i < types.Length; i++)
        {
            int index = i; // captured by the lambdas below
            pools[i] = new ObjectPool<LaneHazard>(
                createFunc: () => CreateHazard(index),
                actionOnGet: h => h.gameObject.SetActive(true),
                actionOnRelease: h => h.gameObject.SetActive(false),
                actionOnDestroy: h => Destroy(h.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 30);
        }

        if (coinPrefab != null)
        {
            coinPool = new ObjectPool<LaneHazard>(
                createFunc: CreateCoin,
                actionOnGet: h => h.gameObject.SetActive(true),
                actionOnRelease: h => h.gameObject.SetActive(false),
                actionOnDestroy: h => Destroy(h.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 30);
        }
    }

    private void Start()
    {
        if (!IsSetupValid())
        {
            enabled = false;
            return;
        }

        foreach (SpawnType t in types)
            totalWeight += t.weight;

        if (totalWeight <= 0f)
        {
            Debug.LogError("ObstacleSpawner: all weights are 0, so nothing could spawn.", this);
            enabled = false;
            return;
        }

        distanceSinceRow = rowSpacing;
    }

    private bool IsSetupValid()
    {
        if (types == null || types.Length == 0)
        {
            Debug.LogError("ObstacleSpawner: add at least one entry to the Types list.", this);
            return false;
        }

        foreach (SpawnType t in types)
        {
            if (t == null || t.prefab == null)
            {
                Debug.LogError("ObstacleSpawner: a Types entry has no prefab assigned.", this);
                return false;
            }
        }

        if (lanePoints == null || lanePoints.Length < 2)
        {
            Debug.LogError("ObstacleSpawner: assign at least 2 lane points.", this);
            return false;
        }

        return true;
    }

    private void Update()
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
        int count = UnityEngine.Random.Range(minAllowed, maxAllowed + 1);

        laneBuffer.Clear();

        for (int i = 0; i < laneCount; i++) 
            laneBuffer.Add(i);

        for (int i = 0; i < count; i++)
        {
            int j = UnityEngine.Random.Range(i, laneCount);
            int lane = laneBuffer[j];
            laneBuffer[j] = laneBuffer[i];
            laneBuffer[i] = lane;

            int typeIndex = PickTypeIndex();
            LaneHazard h = pools[typeIndex].Get();
            h.Launch(new Vector3(lanePoints[lane].position.x, y, 0f), despawnY);
        }

        if (coinPool != null && count < laneCount && UnityEngine.Random.value < coinChance)
        {
            int freeLane = laneBuffer[UnityEngine.Random.Range(count, laneCount)];
            LaneHazard coin = coinPool.Get();
            coin.Launch(new Vector3(lanePoints[freeLane].position.x, y, 0f), despawnY);
        }
    }

    private int PickTypeIndex()
    {
        float roll = UnityEngine.Random.value * totalWeight;

        for (int i = 0; i < types.Length; i++)
        {
            roll -= types[i].weight;

            if (roll <= 0f)
                return i;
        }

        return types.Length - 1;
    }

    private LaneHazard CreateCoin()
    {
        LaneHazard c = Instantiate(coinPrefab, transform);
        c.Setup(coin => coinPool.Release(coin));
        return c;
    }

    private LaneHazard CreateHazard(int index)
    {
        LaneHazard h = Instantiate(types[index].prefab, transform);
        h.Setup(hazard => pools[index].Release(hazard));
        return h;
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
                if (t == null)
                    continue;

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
