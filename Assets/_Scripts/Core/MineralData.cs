using UnityEngine;

// This defines the types of minerals available
public enum MineralType
{
    Gold,
    FoolsGold,
    Coal,
    Iron,
    Amethyst,
    Emerald,
    Sapphire,
    Diamond
}

public class MineralData : MonoBehaviour
{
    public MineralType type;
}