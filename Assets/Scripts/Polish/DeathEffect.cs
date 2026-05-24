using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public static DeathEffect Instance { get; private set; }

    [SerializeField] private GameObject deathParticlePrefab;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color sprinterColor = Color.yellow;
    [SerializeField] private Color tankColor = Color.red;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayDeathEffect(Vector3 position, int zombieTypeIndex)
    {
        if (deathParticlePrefab == null) return;

        GameObject effect = Instantiate(
            deathParticlePrefab, position, Quaternion.identity);

        // set color based on zombie type
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            var main = ps.main;
            main.startColor = zombieTypeIndex switch
            {
                0 => normalColor,
                1 => sprinterColor,
                2 => tankColor,
                _ => Color.white
            };
        }

        Destroy(effect, 2f);
    }
}