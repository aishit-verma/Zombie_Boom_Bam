using UnityEngine;

public class Grenade : MonoBehaviour
{
    private GrenadeSO grenadeData;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float travelTimer;
    private bool hasLanded = false;
    private float fuseTimer;

    [SerializeField] private GameObject explosionEffectPrefab;

    public void Initialize(GrenadeSO data, Vector3 target)
    {
        grenadeData = data;
        startPosition = transform.position;
        targetPosition = target;
        travelTimer = data.travelTime;
        fuseTimer = data.fuseTime;
    }

    void Update()
    {
        if (!hasLanded)
            Travel();
        else
            CountdownFuse();
    }

    private void Travel()
    {
        travelTimer -= Time.deltaTime;
        float t = 1f - (travelTimer / grenadeData.travelTime);

        // arc movement
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        // simple scale pulse while travelling
        float scale = 1f + Mathf.Sin(t * Mathf.PI) * 0.3f;
        transform.localScale = Vector3.one * scale;

        if (travelTimer <= 0f)
        {
            transform.position = targetPosition;
            hasLanded = true;
        }
    }

    private void CountdownFuse()
    {
        fuseTimer -= Time.deltaTime;

        // pulse faster as fuse runs out
        float pulse = Mathf.PingPong(Time.time * 10f, 1f);
        transform.localScale = Vector3.one * (0.8f + pulse * 0.4f);

        if (fuseTimer <= 0f)
            Explode();
    }

    private void Explode()
    {
        AudioManager.Instance?.PlayGrenadeExplosion();
        
        // find all zombies in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            grenadeData.explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // damage
                hit.GetComponent<ZombieController>()
                   ?.TakeDamage(grenadeData.explosionDamage);

                // knockback
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockbackDir =
                        (hit.transform.position - transform.position).normalized;
                    rb.AddForce(knockbackDir * grenadeData.knockbackForce,
                        ForceMode2D.Impulse);
                }
            }
            CameraShake.Instance.Shake(0.5f);
        }

        // spawn explosion effect later
        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, transform.position,
                Quaternion.identity);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // visualize explosion radius in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 
            grenadeData != null ? grenadeData.explosionRadius : 1f);
    }
}