using UnityEngine;

public class Simulation : MonoBehaviour
{
    [SerializeField] float tickSeconds = 1f;
    float t;
    BuildingView[] buildings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {buildings = FindObjectsByType<BuildingView>(FindObjectsSortMode.None);}

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t < tickSeconds) return;
        t = 0f;

        foreach (var b in buildings)
        {
            if (!b) continue;
            //turn per-minute into per-tick
            float perSecond = b.OutputPerMinute / 60f;
            int income = Mathf.RoundToInt(perSecond * tickSeconds);
            if (income > 0) Economy.I.Add(income);
        }   
    }
}
