using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/ZombieType")]
public class ZombieTypeSO : ScriptableObject
{
    [Header("Info")]
    public string zombieName;
    public GameObject zombiePrefab;
    public Sprite zombieSprite; 

    [Header("Stats")]
    public float health;
    public float moveSpeed;
    public float contactDamage;

    [Header("Drops")]
    public DropEntry[] possibleDrops;
    public float dropChance;
    public int creditDropAmount;
    public float healthDropAmount;
}

[System.Serializable]
public class DropEntry
{
    public DropSO drop;
    [Range(0f, 1f)] public float weight;
}