using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance { get; private set; }

    [Header("Credits & Wave")]
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Health")]
    [SerializeField] private Slider healthBar;

    [Header("Weapon Slots")]
    [SerializeField] private Image[] weaponSlotsBackground = new Image[3];
    [SerializeField] private Image[] weaponIcons = new Image[3];
    [SerializeField] private TextMeshProUGUI[] weaponKeyTexts = new TextMeshProUGUI[3];

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI ammoText;

    private int activeSlot = 0;

    [Header("Slot Colors")]
    [SerializeField] private Color normalSlotColor = new Color(0.3f, 0.3f, 0.3f, 1f);
    [SerializeField] private Color activeSlotColor = new Color(1f, 0.8f, 0f, 1f);
    [SerializeField] private Color emptyIconColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    [Header("Grenade Slot")]
    [SerializeField] private Image grenadeSlotBackground;
    [SerializeField] private Image grenadeIcon;
    [SerializeField] private TextMeshProUGUI grenadeKeyText;

    [Header("Grenade Count")]
    [SerializeField] private TextMeshProUGUI grenadeCountText;

    [Header("Grenade Slot Colors")]
    [SerializeField]
    private Color grenadeAvailableColor =
        new Color(0.2f, 0.8f, 0.2f, 1f);
    [SerializeField]
    private Color grenadeEmptyColor =
        new Color(0.5f, 0.5f, 0.5f, 1f);

    public void UpdateGrenades(int current, int max)
    {
        if (grenadeCountText != null)
            grenadeCountText.text = $"Grenades: {current}/{max}";

        if (grenadeSlotBackground != null)
            grenadeSlotBackground.color = current > 0 ?
                grenadeAvailableColor : grenadeEmptyColor;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializeWeaponSlots();
    }

    void Start()
    {
        EconomyManager.Instance.OnCreditsChanged += UpdateCredits;
        WaveManager.Instance.OnWaveStart += UpdateWaveUI;

        UpdateCredits(EconomyManager.Instance.GetCredits());
        UpdateWaveUI();
        UpdateHealth(1f);
        UpdateAmmo(0, 0);

    }

    void OnDestroy()
    {
        EconomyManager.Instance.OnCreditsChanged -= UpdateCredits;
        WaveManager.Instance.OnWaveStart -= UpdateWaveUI;
    }

    // ── Credits & Wave ──────────────────────────────
    public void UpdateCredits(int credits) =>
        creditsText.text = $"Credits: {credits}";

    public void UpdateWaveUI() =>
        waveText.text = $"Wave: {WaveManager.Instance.GetCurrentWaveNumber()}";

    // ── Health ──────────────────────────────────────
    public void UpdateHealth(float healthPercent) =>
        healthBar.value = healthPercent;

    // ── Ammo ────────────────────────────────────────
    public void UpdateAmmo(int current, int max)
    {
        if (max == 0)
            ammoText.text = "-- / --";
        else if (current == -1)
            ammoText.text = "Reloading...";  // -1 = reloading signal
        else
            ammoText.text = $"{current} / {max}";
    }

    // ── Weapon Slots ────────────────────────────────
    private void InitializeWeaponSlots()
    {
        for (int i = 0; i < weaponSlotsBackground.Length; i++)
        {
            if (weaponKeyTexts[i] != null)
                weaponKeyTexts[i].text = (i + 1).ToString();

            if (weaponIcons[i] != null)
                weaponIcons[i].color = emptyIconColor;
        }

        SetActiveSlot(0);

        if (grenadeKeyText != null)
            grenadeKeyText.text = "E";

        if (grenadeSlotBackground != null)
            grenadeSlotBackground.color = normalSlotColor;

        if (grenadeCountText != null)
            grenadeCountText.text = "Grenades: 0/0";
    }

    public void SetActiveSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlotsBackground.Length) return;

        for (int i = 0; i < weaponSlotsBackground.Length; i++)
        {
            if (weaponSlotsBackground[i] != null)
                weaponSlotsBackground[i].color = normalSlotColor;
        }

        activeSlot = slotIndex;
        if (weaponSlotsBackground[activeSlot] != null)
            weaponSlotsBackground[activeSlot].color = activeSlotColor;
    }

    public void SetWeaponInSlot(int slotIndex, Sprite icon, Color weaponColor)
    {
        if (slotIndex < 0 || slotIndex >= weaponIcons.Length) return;



        if (weaponIcons[slotIndex] != null)
        {

            weaponIcons[slotIndex].color = weaponColor;
            if (icon != null)
                weaponIcons[slotIndex].sprite = icon;
        }

    }

    public void ClearWeaponSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponIcons.Length) return;

        if (weaponIcons[slotIndex] != null)
        {
            weaponIcons[slotIndex].sprite = null;
            weaponIcons[slotIndex].color = emptyIconColor;
        }
    }
}