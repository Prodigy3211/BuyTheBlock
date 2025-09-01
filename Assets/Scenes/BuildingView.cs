using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class BuildingView : MonoBehaviour, IPointerClickHandler
{
    public BuildingType type;
    public int level = 0;
    public float health = 1f;
    public bool Owned => level > 0;

    public int PurchaseCost => type.baseCost;
    public int UpgradeCost => Mathf.RoundToInt(type.baseCost * Mathf.Pow(type.upgradeFactor, Mathf.Max(0, level)));
    public int CostPerRepair => Mathf.RoundToInt(type.baseCost * 0.25f);

    public int RepairCost
    {
        get
        {
            float missing = 1f - Mathf.Clamp01(health);
            return Mathf.Max(1, Mathf.RoundToInt(type.baseCost * missing));
        }
    }
    public float OutputPerMinute => Owned ? type.baseOutputPerMinute * Mathf.Max(1, level) * health : 0f;

    public void OnPointerClick(PointerEventData e)
    {
        UIManager.Instance.Open(this);
    }

    public bool TryPurchase()
    {
        if (Owned) return false;
        if (!Economy.I.Spend(UpgradeCost)) return false;
        level = 1;
        return true;
    }

    public bool TryUpgrade()
    {
        if (!Owned) return false;
        if (!Economy.I.Spend(UpgradeCost)) return false;
        level++;
        return true;
    }

    public bool TryRepair()
    {
        if (!Owned) return false; //Can't repair unless you own the property
        if (health >= 1f) return false; //Full Health can't repair 
        if (!Economy.I.Spend(CostPerRepair)) return false;

        health = Mathf.Clamp01(health + 0.25f); //Is this value reasonable?
        return true;
    }
}
