using R3;
using System;
using TMPro;
using UnityEngine;

public class EconomySystem : MonoBehaviour, ISaveLoad
{
    public ReactiveProperty<long> Gold = new ReactiveProperty<long>(0);
    public ReactiveProperty<int> Diamond = new ReactiveProperty<int>(0);
    [SerializeField] TextMeshProUGUI txtGold, txtDiamond;
    private void Awake()
    {
        ServiceLocator.Register(this);
        Load();
        Gold.Subscribe(GoldValue =>
        {
            txtGold.text = GoldValue.FormatNumber();
            Save();
        }).AddTo(this);
        Diamond.Subscribe(DiamondValue =>
        {
            txtDiamond.text = DiamondValue.ToString();
            Save();
        }).AddTo(this);
        txtGold.text = Gold.Value.FormatNumber();
        txtDiamond.text = Diamond.Value.ToString();
    }
    public bool TryPurchaseGold(long amount, out long gold)
    {
        if (Gold.Value >= amount)
        {
            Gold.Value -= amount;
            gold = Gold.Value;
            return true;
        }
        gold = Gold.Value;
        return false;
    }
    public void Load()
    {
        Gold.Value = long.Parse(PlayerPrefs.GetString("Gold", 1000.ToString()));
        Diamond.Value = PlayerPrefs.GetInt("Diamond", 0);
    }

    public void Save()
    {
        PlayerPrefs.SetString("Gold", Gold.Value.ToString());
        PlayerPrefs.SetInt("Diamond", Diamond.Value);
        PlayerPrefs.Save();
    }
    [ContextMenu("Fake Gold")]
    public void FakeGold()
    {
        Gold.Value = 1_000_000_000_000;
    }
}
public static partial class Extensions
{
    public static string FormatNumber(this long num)
    {
        if (num >= 1_000_000_000_000)
            return (num / 1_000_000_000_000D).ToString("0.##T");
        if (num >= 1_000_000_000)
            return (num / 1_000_000_000D).ToString("0.##B"); // Billions
        if (num >= 1_000_000)
            return (num / 1_000_000D).ToString("0.##M");
        if (num >= 1_000)
            return (num / 1_000D).ToString("0.##K");

        return num.ToString("#,0");
    }
}