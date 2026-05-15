using UnityEngine;

public class Drop : MonoBehaviour
{
    private DropSO dropData;
    private int creditAmount;
    private float healthAmount;

    public void Initialize(DropSO data, int creditOverride, float healthOverride)
    {
        dropData = data;
        creditAmount = creditOverride;
        healthAmount = healthOverride;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        switch (dropData.dropType)
        {
            case DropType.Credits:
                EconomyManager.Instance.AddCredits(creditAmount);
                break;

            case DropType.HealthPack:
                PlayerHealth.Instance.Heal(healthAmount);
                break;
        }

        Destroy(gameObject);
    }
}