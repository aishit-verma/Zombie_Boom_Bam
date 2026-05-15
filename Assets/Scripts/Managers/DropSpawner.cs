using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    public static DropSpawner Instance { get; private set; }

    [SerializeField] private GameObject creditsDropPrefab;
    [SerializeField] private GameObject healthPackPrefab;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnDrop(DropSO drop, Vector3 position, 
                          int creditAmount, float healthAmount)
    {
        GameObject prefab = drop.dropType switch
        {
            DropType.Credits    => creditsDropPrefab,
            DropType.HealthPack => healthPackPrefab,
            _                   => null
        };

        if (prefab == null) return;

        GameObject dropObj = Instantiate(prefab, position, Quaternion.identity);
        dropObj.GetComponent<Drop>().Initialize(drop, creditAmount, healthAmount);
    }
}