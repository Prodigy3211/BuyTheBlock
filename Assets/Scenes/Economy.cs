using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy I;
    public int cash = 500;
    void Awake() { I = this; }
    public bool Spend(int amt) { if (cash < amt) return false; cash -= amt; return true; }
    public void Add(int amt) { cash += amt; } 
}
