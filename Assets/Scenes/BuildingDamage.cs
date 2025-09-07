using UnityEngine;
using UnityEngine.UI;

public class BuildingDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float currentHealth;
    float maxHealth = 100f;
    float decayDamage = 1f;
    public Slider buildingHealth;

    void Start()
    {
        currentHealth = maxHealth;
        buildingHealth.maxValue = maxHealth;
        buildingHealth.value = currentHealth;
    }
    void Update()
    {
        currentHealth -= decayDamage * Time.deltaTime;
        currentHealth = Mathf.Max(currentHealth, 0f);
        buildingHealth.value = currentHealth; 
    }
}


