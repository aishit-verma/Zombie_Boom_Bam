using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; }

    [SerializeField] private ShopItemUI[] itemSlots;    // 3 item slots
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button closeButton;

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

    public void Show(ShopItemSO[] items)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;        // pause game during shop

        titleText.text = "SHOP";

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Length && items[i] != null)
                itemSlots[i].Setup(items[i]);
            else
                itemSlots[i].SetEmpty();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    // close button in inspector → OnClick → ShopUI.OnCloseClicked
    public void OnCloseClicked() => Hide();
}