using UnityEngine;

[CreateAssetMenu(menuName = "Game/Building Type")]
public class BuildingType : ScriptableObject
{
    public string displayName = "Shop";
    public Sprite sprite;
    public int baseCost = 100;
    public int baseOutputPerMinute = 10;
    [Range(1.1f, 3f)] public float upgradeFactor = 1.6f;

}
