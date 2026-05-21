using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private float lifetime;
    private bool hasHit = false;

    public void Initialize(float damage, float speed, float lifetime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (hasHit) return;
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // check for DOTS zombie at current position
        bool hit = ZombieHitHandler.Instance
            .DamageNearestZombie(transform.position, damage, 0.5f);

        if (hit)
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}