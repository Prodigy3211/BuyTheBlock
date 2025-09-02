using UnityEngine;

[CreateAssetMenu(menuName = "Game/Building Type")]
public class BuildingType : ScriptableObject
{
    [Header("Identity")]
    public string displayName = "Shop";
    public Sprite sprite;

    [Header("Economy")]
    public int baseCost = 100;
    public int baseOutputPerMinute = 10;
    [Range(1.1f, 3f)] public float upgradeFactor = 1.6f;

    [Header("Health")]
    public int maxHealth = 100;
    public int repairChunk = 25;
    public float decayPerMinute = 3f;

}
