using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed;
    private float damage;
    private float lifetime;

    public void Initialize(float damage, float speed, float lifetime)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifetime = lifetime;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<ZombieController>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}