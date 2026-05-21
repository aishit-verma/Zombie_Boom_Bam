using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private ZombieTypeSO zombieType;
    private float currentHealth;
    private bool isDead = false;

    private Rigidbody2D rb;
    private Transform player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public void Initialize(ZombieTypeSO type)
    {
        zombieType = type;
        currentHealth = type.health;
    }

    void FixedUpdate()
    {
        ChasePlayer();
    }

    private void ChasePlayer()
    {
        if (player == null) return;
        Vector2 direction =
            (player.position - transform.position).normalized;
        rb.linearVelocity = direction * zombieType.moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerHealth.Instance.TakeDamage(zombieType.contactDamage);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerHealth.Instance.TakeDamage(
                zombieType.contactDamage * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        TrySpawnDrop();
        WaveManager.Instance.OnZombieDied();
        Destroy(gameObject);
    }

    private void TrySpawnDrop()
    {
        if (zombieType.possibleDrops.Length == 0) return;
        if (Random.value > zombieType.dropChance) return;

        // weight based drop selection
        float roll = Random.value;
        float cumulative = 0f;

        foreach (var entry in zombieType.possibleDrops)
        {
            cumulative += entry.weight;
            if (roll <= cumulative)
            {
                DropSpawner.Instance.SpawnDrop(
                    entry.drop,
                    transform.position,
                    zombieType.creditDropAmount,
                    zombieType.healthDropAmount);
                return;
            }
        }
    }
}