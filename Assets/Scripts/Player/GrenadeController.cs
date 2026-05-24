using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public static GrenadeController Instance { get; private set; }

    [SerializeField] private GrenadeSO grenadeData;
    [SerializeField] private GameObject grenadePrefab;

    private int currentCount;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        currentCount = grenadeData.maxCount;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentCount > 0)
            ThrowGrenade();
    }

    private void ThrowGrenade()
    {
        // calculate target in aim direction at fixed range
        Vector3 mouseWorldPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection =
            (mouseWorldPos - transform.position).normalized;
        Vector3 targetPosition =
            transform.position + (Vector3)(aimDirection * grenadeData.throwRange);

        // spawn grenade
        GameObject grenadeObj = Instantiate(
            grenadePrefab,
            transform.position,
            Quaternion.identity);

        grenadeObj.GetComponent<Grenade>()
                  .Initialize(grenadeData, targetPosition);

        currentCount--;
        UpdateUI();
     
    }

    public void AddGrenades(int amount)
    {
        currentCount = Mathf.Min(currentCount + amount,
            grenadeData.maxCount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        GameplayUI.Instance.UpdateGrenades(currentCount, grenadeData.maxCount);
    }



    public int GetCurrentCount() => currentCount;
    public int GetMaxCount() => grenadeData.maxCount;
}