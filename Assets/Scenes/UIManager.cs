using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Refs")]
    public GameObject panel;
    public TMP_Text title, level, output, cashLabel;
    public Button buyBtn, upgradeBtn, repairBtn;
    BuildingView current;

    void Awake() { Instance = this; }
    void Update() { if (cashLabel) cashLabel.text = $"${Economy.I.cash}"; }

    public void Open(BuildingView view)
    {
        current = view;
        panel.SetActive(true);
        Refresh();
    }
    public void Close() { panel.SetActive(false); current = null; }

    void Refresh()
    {
        if (current == null) return;
        title.text = current.type.displayName;
        level.text = $"Level {current.level}";
        output.text = $"{current.OutputPerMinute:0}/min";
        buyBtn.gameObject.SetActive(current.level == 0);
        upgradeBtn.gameObject.SetActive(current.level > 0);

        buyBtn.onClick.RemoveAllListeners();
        buyBtn.onClick.AddListener(() =>
        {
            if (Economy.I.Spend(current.PurchaseCost))
            {
                current.level = 1;
                Refresh();
            }
        });

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(() =>
        {
            if (Economy.I.Spend(current.UpgradeCost))
            {
                current.level++;
                Refresh();
            }
        });

        repairBtn.onClick.RemoveAllListeners();
        repairBtn.onClick.AddListener(() =>
        {
            if (Economy.I.Spend(25)) { current.health = 1f; Refresh(); }
        });

    }
}
