using UnityEngine;

[CreateAssetMenu(menuName = "Throwables/Grenade")]
public class GrenadeSO : ScriptableObject
{
    [Header("Info")]
    public string grenadeName;
    public int maxCount;            // how many player can carry

    [Header("Throw")]
    public float throwRange;        // fixed distance in aim direction
    public float travelTime;        // time to reach target

    [Header("Explosion")]
    public float explosionRadius;   // area of effect
    public float explosionDamage;   // damage to zombies
    public float knockbackForce;    // push force on zombies

    [Header("Timing")]
    public float fuseTime;          // delay before explosion after landing
}