using UnityEngine;

public class ZombieDropBridge : MonoBehaviour
{
    public static ZombieDropBridge Instance { get; private set; }

    [SerializeField] private ZombieTypeSO[] zombieTypes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TrySpawnDrop(Vector3 position, int typeIndex)
    {
        ZombieTypeSO zombieType =
            ZombieVisualRegistry.Instance.GetZombieType(typeIndex);

        if (zombieType == null) return;
        if (zombieType.possibleDrops.Length == 0) return;
        if (Random.value > zombieType.dropChance) return;

        float roll = Random.value;
        float cumulative = 0f;

        foreach (var entry in zombieType.possibleDrops)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
            {
                DropSpawner.Instance.SpawnDrop(
                    entry.drop,
                    position,
                    zombieType.creditDropAmount,
                    zombieType.healthDropAmount);
                return;
            }
        }
    }
    public ZombieTypeSO GetZombieType(int index)
    {
        if (index < 0 || index >= zombieTypes.Length) return null;
        return zombieTypes[index];
    }
}