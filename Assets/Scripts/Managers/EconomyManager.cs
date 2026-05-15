using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private int startingCredits = 100;
    private int currentCredits;

    public System.Action<int> OnCreditsChanged; // UI listens to this

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentCredits = startingCredits;
    }

    void Start()
    {
        // Listen to wave end event
        WaveManager.Instance.OnWaveEnd += GiveWaveBonus;
    }

    void OnDestroy()
    {
        WaveManager.Instance.OnWaveEnd -= GiveWaveBonus;
    }

    private void GiveWaveBonus()
    {
        // Get current wave config reward
        int bonus = WaveManager.Instance.GetCurrentWaveReward();
        AddCredits(bonus);
        Debug.Log($"Wave bonus! +{bonus} credits");
    }

    public void AddCredits(int amount)
    {
        currentCredits += amount;
        OnCreditsChanged?.Invoke(currentCredits);
        Debug.Log($"Credits: {currentCredits}");
    }

    public bool SpendCredits(int amount)
    {
        if (currentCredits < amount)
        {
            Debug.Log("Not enough credits!");
            return false;
        }
        currentCredits -= amount;
        OnCreditsChanged?.Invoke(currentCredits);
        Debug.Log($"Spent {amount}. Credits remaining: {currentCredits}");
        return true;
    }

    public int GetCredits() => currentCredits;
}