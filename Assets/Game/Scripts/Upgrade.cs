using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UpgradeType
{
    KICK,
    BONUS,
    STACK
}

public class Upgrade : MonoBehaviour
{
    public UpgradeType upgrade_type;
    public int price_divider;
    public int price_multipler;

    public TextMeshProUGUI price_txt;
    public TextMeshProUGUI level_txt;
    public TextMeshProUGUI resource_txt;
    public TextMeshProUGUI upgrade_txt;


    string upgrade_name;

    private void Start()
    {
        upgrade_name = upgrade_type.ToString();
        UpdateUI(GetLevel(), GetPrice());
    }

    public void LevelUp()
    {
        int current_level = GetLevel();
        current_level++;
        PlayerPrefs.SetInt(upgrade_name, current_level);
        UpdateUI(GetLevel(), GetPrice());
    }

    private int GetLevel()
    {
        return PlayerPrefs.GetInt(upgrade_name, 1);
    }

    private int GetPrice()
    {
        int current_level = GetLevel();
        return ((current_level * current_level) / price_divider)* price_multipler;
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
        switch (upgrade_type)
        {
            case UpgradeType.KICK:

                resource_txt.SetText("x" + level * 5);
                upgrade_txt.SetText("+" + 5);
                level_txt.SetText("" + level);
                price_txt.SetText(price + "$");

                break;
            case UpgradeType.BONUS:
                resource_txt.SetText("x" + (1 + level * 0.2f));
                upgrade_txt.SetText("+" + 0.2);
                level_txt.SetText("" + level);
                price_txt.SetText(price + "$");

                break;
            case UpgradeType.STACK:
                resource_txt.SetText(""+level);
                upgrade_txt.SetText("+" + 1);
                level_txt.SetText("" + level);
                price_txt.SetText(price + "$");
                break;
            default:
                break;
        }
    }
}
