using UnityEngine;

public enum ShopItemType { Weapon, HealthPack, Ammo, Grenade }

[CreateAssetMenu(menuName = "Shop/ShopItem")]
public class ShopItemSO : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public Sprite itemIcon;
    public ShopItemType itemType;
    public int cost;

    [Header("Weapon")]
    public WeaponSO weapon;         // if itemType = Weapon

    [Header("Health")]
    public float healAmount;        // if itemType = HealthPack

    [Header("Ammo")]
    public int ammoAmount;          // if itemType = Ammo
    [Header("Grenade")]
    public int grenadeAmount;
}