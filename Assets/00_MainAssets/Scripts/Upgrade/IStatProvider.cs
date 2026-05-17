using R3;
using UnityEngine;
// Base interface
public interface IStatProvider
{
    float GetValue();
}
public class StatProviderRef
{
    public IStatProvider Value;
}

// Base stat
public class BaseStat : IStatProvider
{
    private float _baseValue;
    public BaseStat(float value) => _baseValue = value;
    public float GetValue() => _baseValue;
}

// Decorator base
public abstract class StatUpgrade : IStatProvider
{
    protected IStatProvider _inner;
    public StatUpgrade(IStatProvider inner) => _inner = inner;
    public abstract float GetValue();
}

// Flat bonus upgrade
public class FlatUpgrade : StatUpgrade
{
    private float _bonus;
    public FlatUpgrade(IStatProvider inner, float bonus) : base(inner) => _bonus = bonus;
    public override float GetValue() => _inner.GetValue() + _bonus;
}

// Percent multiplier upgrade
public class PercentUpgrade : StatUpgrade
{
    private float _multiplier;
    public PercentUpgrade(IStatProvider inner, float multiplier) : base(inner) => _multiplier = multiplier;
    public override float GetValue() => _inner.GetValue() * _multiplier;
}
public class OverrideUpgrade : StatUpgrade
{
    private float _overrideValue;
    public OverrideUpgrade(IStatProvider inner, float overrideValue) : base(inner) => _overrideValue = overrideValue;
    public override float GetValue() => _overrideValue;
}
[System.Serializable]
public struct StatModifier
{
    public ModierType modType;
    public float value;
}
public enum ModierType
{
    Additive,
    Multiplier,
    Override
}
public static partial class Extensions
{
    public static void ApplyStats(StatProviderRef statProviderRef, GameObject gameObject, params UpgradeSO[] upgradeSO)
    {
        foreach (var item in upgradeSO)
        {
            if (item.isUnlock.Value)
            {
                ApplyStats(ref statProviderRef.Value, item.statModifier);
            }
            else
            {
                item.isUnlock.Subscribe(itemUnlocked =>
                {
                    if (itemUnlocked)
                    {
                        ApplyStats(ref statProviderRef.Value, item.statModifier);
                    }
                }).AddTo(gameObject);
            }
        }
        void ApplyStats(ref IStatProvider baseStat, StatModifier statModifier)
        {
            switch (statModifier.modType)
            {
                case ModierType.Additive:
                    baseStat = new FlatUpgrade(baseStat, statModifier.value);
                    break;
                case ModierType.Multiplier:
                    baseStat = new PercentUpgrade(baseStat, statModifier.value);
                    break;
                case ModierType.Override:
                    baseStat = new OverrideUpgrade(baseStat, statModifier.value);
                    break;
                default:
                    break;
            }
        }
    }
}