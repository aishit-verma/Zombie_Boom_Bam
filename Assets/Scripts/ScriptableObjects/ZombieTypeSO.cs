using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/ZombieType")]
public class ZombieTypeSO : ScriptableObject
{
    [Header("Info")]
    public string zombieName;
    public GameObject zombiePrefab;    // ← each type has its own prefab

    [Header("Stats")]
    public float health;
    public float moveSpeed;
    public float contactDamage;

    [Header("Drops")]
    public DropSO[] possibleDrops;
    public float dropChance;
    public int creditDropAmount;    // override credits per zombie type
    public float healthDropAmount;  // override health per zombie type
}