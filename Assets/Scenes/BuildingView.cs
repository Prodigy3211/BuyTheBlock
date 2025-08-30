using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class BuildingView : MonoBehaviour, IPointerClickHandler
{
    public BuildingType type;
    public int level = 0;
    public float health = 1f;

    public int PurchaseCost => type.baseCost;
    public int UpgradeCost => Mathf.RoundToInt(type.baseCost * Mathf.Pow(type.upgradeFactor, Mathf.Max(0, level)));
    public float OutputPerMinute => type.baseOutputPerMinute * Mathf.Max(1, level) * health;

    public void OnPointerClick(PointerEventData e)
    {
        UIManager.Instance.Open(this);
    }
}
