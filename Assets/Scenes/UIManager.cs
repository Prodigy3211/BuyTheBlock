using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Panel")]
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text title, level, output, cashLabel;
    [SerializeField] Button buyBtn, upgradeBtn, repairBtn;
    [SerializeField] HealthBarController healthUI;
    [SerializeField] TMP_Text message;
    BuildingView current;
    Coroutine msgCo;

    void Awake() { Instance = this; }
    void Start() { if (panel) panel.SetActive(false); }
    void Update()
    {
        if (cashLabel && Economy.I != null)
            cashLabel.text = $"${Economy.I.cash}";
    }

    // public bool IsOpen => panel && panel.activeSelf;

    public void Open(BuildingView view)
    {
        current = view;
        panel.SetActive(true);
        Refresh();
    }
    public void Close() { panel.SetActive(false); current = null; }

   public void Refresh()
    {
        if (!current) return;
        title.text = current.type.displayName;
        level.text = $"Level {current.level}";
        output.text = $"{current.OutputPerMinute:0}/min";

        if (healthUI != null) healthUI.Set01(current.Health01);
        Debug.Log($"[UI] Refresh for {current.name} health01={current.Health01:0.00}");
        bool owned = current.Owned;


        //Buttons
        buyBtn.gameObject.SetActive(!owned);
        upgradeBtn.gameObject.SetActive(owned);
        repairBtn.gameObject.SetActive(owned);

        //Costs?
        buyBtn.interactable = !owned && Economy.I.cash >= current.PurchaseCost;
        upgradeBtn.interactable = owned && Economy.I.cash >= current.UpgradeCost;
        repairBtn.interactable = owned && current.currentHealth < current.type.maxHealth && Economy.I.cash >= current.RepairCost;

        //After you click these buttons what happens?
        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(() =>
        {
            if (current.TryPurchase()) Refresh();
            else NotEnoughCash();
        });

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() =>
        {
            if (Economy.I.Spend(current.UpgradeCost))
            {
                if (current.TryUpgrade()) Refresh();
                else NotEnoughCash();
            }
        });

        repairBtn.onClick.RemoveAllListeners();
        repairBtn.onClick.AddListener(() => { if (current.TryRepair()) Refresh(); else NotEnoughCash(); });

    }

    void ShowMessage(string txt)
    {
        if (message == null) return;
        message.gameObject.SetActive(true);
        message.text = txt;
        StopAllCoroutines();
        StartCoroutine(HideMsgSoon());
    }
    System.Collections.IEnumerator HideMsgSoon()
    {
        yield return new WaitForSeconds(1.5f);
        message.gameObject.SetActive(false);
    }
    void NotEnoughCash()
    {
        if (!message) return;
        message.gameObject.SetActive(true);
        message.text = "Not Enough Cash My Dude..";
        CancelInvoke(nameof(HideMsg));
        Invoke(nameof(HideMsg), 1.2f);
    }

    void HideMsg() => message.gameObject.SetActive(false);

    // IEnumerator Flash(string txt)
    // {
    //     message.text = txt;
    //     message.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(1.2f);
    //     message.gameObject.SetActive(false);
    // }

}
