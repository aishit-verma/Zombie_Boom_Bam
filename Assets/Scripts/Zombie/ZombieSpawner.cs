using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public static ZombieSpawner Instance { get; private set; }

    [SerializeField] private Transform[] spawnPoints;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnWave(WaveConfigSO wave)
    {
        StartCoroutine(SpawnRoutine(wave));
    }

    private IEnumerator SpawnRoutine(WaveConfigSO wave)
    {
        for (int i = 0; i < wave.zombieCount; i++)
        {
            Transform spawnPoint =
                spawnPoints[Random.Range(0, spawnPoints.Length)];

            ZombieTypeSO zombieType =
                wave.zombieTypes[Random.Range(0, wave.zombieTypes.Length)];

            // prefab comes from ZombieTypeSO directly
            GameObject zombie =
                Instantiate(zombieType.zombiePrefab, spawnPoint.position,
                Quaternion.identity);

            zombie.GetComponent<ZombieController>().Initialize(zombieType);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}