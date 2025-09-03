using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] TMP_Text label;

    void Awake()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (slider)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.wholeNumbers = false;
            slider.interactable = false;
        }
    }
    public void Set01(float v)
    {
        v = Mathf.Clamp01(v);
        if (slider) slider.value = v;
        if (label) label.text = $"{Mathf.RoundToInt(v * 100)}%";

        if (!fill) return;
        if (v > 0.6f) fill.color = Color.green;
        else if (v > 0.3f) fill.color = Color.yellow;
        else fill.color = Color.red;
        Debug.Log($"[HealthBar] Set01 -> {v:0.00}");
    }
    
}
