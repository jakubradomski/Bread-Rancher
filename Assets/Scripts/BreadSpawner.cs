using System.Collections.Generic;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    public static BreadSpawner Instance { get; private set; }

    public float BaseSpawnTime = 300f;

    public float MaxSpawnTimer = 60f;

    public int BreadCap = 200;

    public int BreadCount = 0;

    public float spawnerCountDivisor = 32;

    public float timerPublic;

    public GameObject BreadPrefab;

    public List<GameObject> Spawners;

    private float spawnTimer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timerPublic = BaseSpawnTime / (Spawners.Count / spawnerCountDivisor);
        timerPublic = Mathf.Min(timerPublic, MaxSpawnTimer);

        if (Spawners.Count > 0)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer < 0)
            {
                spawnTimer = BaseSpawnTime / (Spawners.Count / spawnerCountDivisor);
                spawnTimer = Mathf.Min(MaxSpawnTimer, spawnTimer);

                if (BreadCount < BreadCap)
                {
                    BreadCount++;
                    SpawnBread();
                }
            }
        }
    }

    public void SpawnBread()
    {
        if(Spawners.Count > 0)
        {
            int index = Random.Range(0, Spawners.Count);
            Vector3 spawnPos = Spawners[index].transform.position;
            Instantiate(BreadPrefab, spawnPos, Quaternion.identity);
        }
    }
}
