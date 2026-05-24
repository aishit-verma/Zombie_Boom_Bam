using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float zombieDPS = 10f; // damage per second

    private float currentHealth;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentHealth = maxHealth;
    }

    // Fires once on first contact
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            TakeDamage(zombieDPS);
    }

    // Fires every physics step while still in contact
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            TakeDamage(zombieDPS * Time.fixedDeltaTime);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GameplayUI.Instance.UpdateHealth(GetHealthPercent());
   
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        GameplayUI.Instance.UpdateHealth(GetHealthPercent());
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        AudioManager.Instance?.PlayGameOverMusic();
        PlayerMovement.Instance.IsMovementEnabled = false;
        WeaponController.Instance.enabled = false;
        // GameOverUI.Instance.Show();
        gameObject.SetActive(false);
    }

    public float GetHealthPercent() => currentHealth / maxHealth;
}