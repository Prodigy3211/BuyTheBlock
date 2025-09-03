using UnityEngine;
using System.Collections.Generic;

public class Simulation : MonoBehaviour
{
    [SerializeField] float tickSeconds = 1f;
    float t;
    BuildingView[] buildings;
    Dictionary<BuildingView, float> carry = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildings = FindObjectsByType<BuildingView>(FindObjectsSortMode.None);
        foreach (var b in buildings) carry[b] = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t < tickSeconds) return;
        float dt = t;
        t = 0f;

        int total = 0;
        bool anyHealthChanged = false;


        foreach (var b in buildings)
        {
            if (b == null || !b.Owned || b.currentHealth <= 0) continue;
            //turn per-minute into per-tick
            float perSecond = b.OutputPerMinute / 60f;
            float raw = perSecond * dt + carry[b];
            int whole = Mathf.FloorToInt(raw);
            carry[b] = raw - whole;
            total += whole;

            //decay for buildings
            if (b.ApplyDecay(dt)) anyHealthChanged = true;

            // int income = Mathf.RoundToInt(perSecond * tickSeconds);
            // if (income > 0) Economy.I.Add(income);
        }
        if (total > 0) Economy.I.Add(total);
        if (anyHealthChanged && UIManager.Instance != null)
            UIManager.Instance.Refresh();
    }
}
