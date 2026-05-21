using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [SerializeField] private ShopItemSO[] allShopItems;
    [SerializeField] private int itemsToShow = 3;

    private ShopItemSO[] currentShopItems;

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
        WaveManager.Instance.OnShopOpen += OpenShop; 
        WaveManager.Instance.OnWaveStart += CloseShop;
    }

    void OnDestroy()
    {
        WaveManager.Instance.OnShopOpen -= OpenShop;
        WaveManager.Instance.OnWaveStart -= CloseShop;
    }

    private void OpenShop()
    {
        currentShopItems = GetRandomItems();
        ShopUI.Instance.Show(currentShopItems);
    }

    private void CloseShop()
    {
        ShopUI.Instance.Hide();
    }

    private ShopItemSO[] GetRandomItems()
    {
        ShopItemSO[] items = new ShopItemSO[itemsToShow];
        ShopItemSO[] shuffled = (ShopItemSO[])allShopItems.Clone();

        // fisher-yates shuffle
        for (int i = shuffled.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            ShopItemSO temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        for (int i = 0; i < itemsToShow; i++)
            items[i] = shuffled[i];

        return items;
    }

    public bool PurchaseItem(ShopItemSO item, int weaponSlot = 0)
    {
        if (!EconomyManager.Instance.SpendCredits(item.cost))
            return false;

        switch (item.itemType)
        {
            case ShopItemType.Weapon:
                int slot = WeaponController.Instance.GetFirstEmptySlot();
                if (slot != -1)
                {
                    // empty slot found → equip directly
                    WeaponController.Instance.EquipWeapon(item.weapon, slot);
                }
                else
                {
                    // all slots full → ask player which to swap
                    SlotSelectionUI.Instance.Show(item.weapon);
                }
                break;

            case ShopItemType.HealthPack:
                PlayerHealth.Instance.Heal(item.healAmount);
                break;

            case ShopItemType.Ammo:
                Debug.Log($"Ammo refilled: +{item.ammoAmount}");
                break;
            case ShopItemType.Grenade:
                GrenadeController.Instance.AddGrenades(item.grenadeAmount);
                break;


        }

        Debug.Log($"Purchased: {item.itemName}");
        return true;
    }
}