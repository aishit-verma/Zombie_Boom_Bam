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

            int typeIndex = Random.Range(0, wave.zombieTypes.Length);
            ZombieTypeSO zombieType = wave.zombieTypes[typeIndex];

            ZombieDotsSpawner.Instance.SpawnZombie(
                zombieType, spawnPoint.position, typeIndex);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
}