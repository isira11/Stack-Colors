using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class CoinCollector : MonoBehaviour
{
    public static CoinCollector instance;
    public RectTransform MainCanvas;
    public RectTransform target;
    public GameObject CoinsUIPrefab;
    public TextMeshProUGUI coins_txt;

    private void Start()
    {
        instance = this;
        coins_txt.SetText(PlayerPrefs.GetInt("COINS", 0) + "");

    }
    public void AddCoin()
    {
        int level = PlayerPrefs.GetInt(UpgradeType.BONUS.ToString(),1);
        print(""+ level);
        GameObject obj = new GameObject();
        RectTransform t = obj.AddComponent<RectTransform>();
        t.localScale = Vector3.one;
        t.SetParent(MainCanvas);
        t.anchoredPosition = new Vector2(0, -200);

        for (int i = 0; i < level; i++)
        {
            GameObject coin = Instantiate(CoinsUIPrefab);
            RectTransform rt = coin.GetComponent<RectTransform>();
            rt.SetParent(t);
            rt.localPosition = Vector2.zero + new Vector2(Random.Range(-level*2f, level * 2f), Random.Range(-level *2f, level * 2f));
            rt.localScale = Vector3.one;
            rt.sizeDelta = target.sizeDelta;
  
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Append(t.DOMove(target.position, 1.5f).SetEase(Ease.InQuart)).OnComplete(() => {
            int coins = PlayerPrefs.GetInt("COINS", 0);
            coins++;
            PlayerPrefs.SetInt("COINS", coins);
            coins_txt.SetText(coins + "");
            Destroy(obj);
        });

    }
}
