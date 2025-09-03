using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class BuildingView : MonoBehaviour, IPointerClickHandler
{
    public BuildingType type;
    [Header("Instace State")]
    public int level = 0; // Means the building is not owned.
    public int currentHealth;

    float _decayCarry;
    public float Health01 => Mathf.Clamp01(currentHealth);//(float)currentHealth / type.maxHealth);
    public bool Owned => level > 0;

    public int PurchaseCost => type.baseCost;
    public int UpgradeCost => Mathf.RoundToInt(type.baseCost * Mathf.Pow(type.upgradeFactor, Mathf.Max(0, level)));

    public int OutputPerMinute => Owned ? type.baseOutputPerMinute * Mathf.Max(1, level) * (currentHealth > 0 ? 1 : 0) : 0;

    void Awake()
    {
        if (level > 0 && currentHealth <= 0) currentHealth = type.maxHealth; //New ones start at full healthiness!
    }
    public int CostPerRepair => Mathf.RoundToInt(type.baseCost * 0.25f);

    public int RepairCost
    {
        get
        {
            float missing = 1f - Health01;
            return Mathf.Max(1, Mathf.RoundToInt(type.baseCost * missing));
        }
    }
    

    public void OnPointerClick(PointerEventData e)
    {
        UIManager.Instance.Open(this);
    }

    public bool TryPurchase()
    {
        if (level > 0) return false;
        int cost = PurchaseCost;
        if (!Economy.I.Spend(cost)) return false;
        level = 1;
        currentHealth = type.maxHealth;
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
        if (currentHealth >= type.maxHealth) return false; //Full Health can't repair
        int cost = RepairCost;
        if (!Economy.I.Spend(cost)) return false;

        currentHealth = Mathf.Min(type.maxHealth, currentHealth + type.repairChunk);
        return true;
    }
    public void ApplyDecay()
    {

        //100 - 1 every Tick
        if (!Owned || type.decayPerMinute <= 0f || currentHealth <= 0) return false;
        currentHealth -= 1f * Time.deltaTime;
        Debug.Log(currentHealth);
        
        // float perSecond = type.decayPerMinute / 60f;
        // float raw = perSecond * seconds + _decayCarry;

        // int whole = Mathf.FloorToInt(raw);
        // _decayCarry = raw - whole;

        // if (whole <= 0) return false;

        // int before = currentHealth;
        // currentHealth = Mathf.Max(0, currentHealth - whole);
        // return currentHealth != before;
    }
}
