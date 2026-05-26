using UnityEngine;
using UnityEngine.UI;

public class PlayerEconomy : MonoBehaviour
{
    [Header("Налаштування економіки")]
    public int startingGold = 300;
    private int currentGold;

    [Header("UI Інтерфейс")]
    public Text goldText;

    void Start()
    {
        currentGold = startingGold;
        UpdateGoldUI();
    }

    public bool CanAfford(int amount)
    {
        return currentGold >= amount;
    }


    public void SpendGold(int amount)
    {
        if (CanAfford(amount))
        {
            currentGold -= amount;
            UpdateGoldUI();
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }
}