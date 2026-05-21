using UnityEngine;

public class ZombieVisualRegistry : MonoBehaviour
{
    public static ZombieVisualRegistry Instance { get; private set; }

    [SerializeField] private ZombieTypeSO[] zombieTypes;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Sprite GetSprite(int index)
    {
        if (index < 0 || index >= zombieTypes.Length) return null;
        return zombieTypes[index].zombieSprite;
    }

    // public Color GetColor(int index)
    // {
    //     if (index < 0 || index >= zombieTypes.Length) 
    //         return Color.white;
    //     return zombieTypes[index].zombieColor;
    // }

    public ZombieTypeSO GetZombieType(int index)
    {
        if (index < 0 || index >= zombieTypes.Length) return null;
        return zombieTypes[index];
    }
}