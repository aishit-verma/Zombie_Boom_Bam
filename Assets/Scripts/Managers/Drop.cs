using UnityEngine;

public class Drop : MonoBehaviour
{
    private DropSO dropData;
    private int creditAmount;
    private float healthAmount;

    [SerializeField] private float attractRadius = 2f;  // start attracting
    [SerializeField] private float collectRadius = 0.3f; // collect
    [SerializeField] private float attractSpeed = 5f;

    private Transform player;
    private bool isAttracting = false;

    public void Initialize(DropSO data, int creditOverride, 
        float healthOverride)
    {
        dropData = data;
        creditAmount = creditOverride;
        healthAmount = healthOverride;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(
            transform.position, player.position);

        // start attracting when player is close
        if (distance < attractRadius)
            isAttracting = true;

        if (isAttracting)
        {
            // move toward player with increasing speed
            float speed = attractSpeed * 
                (1 + (attractRadius - distance) / attractRadius);

            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime);

            // collect when close enough
            if (distance < collectRadius)
                Collect();
        }
    }

    private void Collect()
    {
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