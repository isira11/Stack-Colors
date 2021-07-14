using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrade 
{
    public string upgrade_name;
    public int price_divider;

    public TextMeshPro price_txt;
    public TextMeshPro level_txt;

    private void Start()
    {
        UpdateUI(GetLevel(), GetPrice());
    }

    public void LevelUp()
    {
        int current_level = GetLevel();
    }

    private int GetLevel()
    {
        return PlayerPrefs.GetInt(upgrade_name, 0);
    }

    private int GetPrice()
    {
        int current_level = GetLevel();
        return (current_level * current_level) / price_divider;
    }

    public bool Transaction(int price)
    {
        int coins = PlayerPrefs.GetInt("COINS", 0);
        if (coins>=price)
        {
            coins -= price;
            PlayerPrefs.GetInt("COINS", coins);
            return true;
        }

        return false;
    }

    public virtual void UpdateUI(int level, int price)
    {
        price_txt.SetText(price + "");
        level_txt.SetText(level + "");
    }
}
