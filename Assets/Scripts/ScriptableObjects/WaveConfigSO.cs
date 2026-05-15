using UnityEngine;

[CreateAssetMenu(menuName = "Waves/WaveConfig")]
public class WaveConfigSO : ScriptableObject
{
    public int zombieCount;
    public float spawnInterval;
    public int creditsReward;
    public ZombieTypeSO[] zombieTypes;  // pool to pick from
}