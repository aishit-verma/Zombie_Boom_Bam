using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;     // ← added
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button buyButton;

    private ShopItemSO currentItem;
    private int weaponSlotIndex = 0;

    public void Setup(ShopItemSO item)
    {
        gameObject.SetActive(true);
        currentItem = item;

        if (item.itemIcon != null)
            itemIcon.sprite = item.itemIcon;

        itemNameText.text = item.itemName;
        descriptionText.text = item.description;
        costText.text = $"{item.cost} Credits";

        bool canAfford =
            EconomyManager.Instance.GetCredits() >= item.cost;
        buyButton.interactable = canAfford;
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void OnBuyClicked()
    {
        if (currentItem == null) return;

        bool success = ShopManager.Instance.PurchaseItem(
            currentItem, weaponSlotIndex);

        if (success)
        {
            buyButton.interactable = false;
            costText.text = "PURCHASED";
        }
    }

    // call this later when you have sprites
    public void SetBackground(Sprite sprite)
    {
        if (backgroundImage != null)
            backgroundImage.sprite = sprite;
    }
}