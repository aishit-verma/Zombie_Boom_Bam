using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotSelectionUI : MonoBehaviour
{
    public static SlotSelectionUI Instance { get; private set; }

    [SerializeField] private Button[] slotButtons = new Button[3];
    [SerializeField] private TextMeshProUGUI[] slotTexts = new TextMeshProUGUI[3];
    [SerializeField] private TextMeshProUGUI titleText;

    private WeaponSO pendingWeapon;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Hide();
    }

    public void Show(WeaponSO weapon)
    {
        gameObject.SetActive(true);
        pendingWeapon = weapon;
        titleText.text = $"Swap {weapon.weaponName} with:";

        // show current weapon names on each button
        for (int i = 0; i < slotButtons.Length; i++)
        {
            WeaponSO equipped = WeaponController.Instance.GetWeaponInSlot(i);
            slotTexts[i].text = equipped != null ? equipped.weaponName : "Empty";
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        pendingWeapon = null;
    }

    // wire each button to these in inspector
    public void OnSlot1Selected() => SelectSlot(0);
    public void OnSlot2Selected() => SelectSlot(1);
    public void OnSlot3Selected() => SelectSlot(2);

    private void SelectSlot(int slot)
    {
        if (pendingWeapon == null) return;
        AudioManager.Instance.PlayButtonClick();
        WeaponController.Instance.EquipWeapon(pendingWeapon, slot);
        Hide();
    }
}