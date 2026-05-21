using UnityEngine;

public enum DropType { Credits, HealthPack }

[CreateAssetMenu(menuName = "Drops/DropData")]
public class DropSO : ScriptableObject
{
    public string dropName;
    public DropType dropType;
}