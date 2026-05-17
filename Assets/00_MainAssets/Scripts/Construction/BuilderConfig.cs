using UnityEngine;

[CreateAssetMenu]
public class BuilderConfig : ScriptableObject
{
    public Sprite icon;
    public string builderName;
    public int baseIncome;
    public int buildCost;
    public int buildCostMultiplier;
    public float buildTime;
    public float growIncome;
    public float harvestTime;
    public float buildTimeMultiplier;
    public int maxLevel;
}
