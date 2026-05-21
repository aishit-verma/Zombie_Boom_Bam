using UnityEngine;

public class ZombieContactBridge : MonoBehaviour
{
    public static ZombieContactBridge Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void DealDamage(float damage)
    {
        PlayerHealth.Instance.TakeDamage(damage);
    }
}