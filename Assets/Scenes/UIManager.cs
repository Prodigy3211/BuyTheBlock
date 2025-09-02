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
    [SerializeField] Slider healthBar;
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

    public bool IsOpen => panel && panel.activeSelf;

    public void Open(BuildingView view)
    {
        current = view;
        panel.SetActive(true);
        Refresh();
    }
    public void Close() { panel.SetActive(false); current = null; }

    void Refresh()
    {
        if (!current) return;
        title.text = current.type.displayName;
        level.text = $"Level {current.level}";
        output.text = $"{current.OutputPerMinute:0}/min";

        //Buttons
        buyBtn.gameObject.SetActive(!current.Owned);
        upgradeBtn.gameObject.SetActive(current.Owned);
        repairBtn.gameObject.SetActive(current.Owned);

        //Costs?
        buyBtn.interactable = !current.Owned && Economy.I.cash >= current.PurchaseCost;
        upgradeBtn.interactable = current.Owned && Economy.I.cash >= current.UpgradeCost;
        repairBtn.interactable = current.Owned && current.health < 1f && Economy.I.cash >= 25;

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
        repairBtn.onClick.AddListener(TryRepair);

        void TryRepair()
        {
            if (current == null) return;

            //Full health already
            if (current.health >= 1f) { ShowMessage("Already At Full Health :)"); return; }

            int cost = current.RepairCost;
            if (!Economy.I.Spend(cost)) { ShowMessage("Not Enough Cash!"); return; }

            // 25% refill
            current.health = Mathf.Min(1f, current.health + 0.25f);

            Refresh();
        }

        if (healthBar) healthBar.value = current ? Mathf.Clamp01(current.health) : 0f;

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
        if (msgCo != null) StopCoroutine(msgCo);
        msgCo = StartCoroutine(Flash("Not enough cash my Dude..."));
    }

    IEnumerator Flash(string txt)
    {
        message.text = txt;
        message.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        message.gameObject.SetActive(false);
    }

}
