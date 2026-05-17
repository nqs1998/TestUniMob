using System;

public class Calculator
{
    public static int CalculateBuildCost(BuilderConfig config, int level)
    {
        var cost = config.buildCost;
        cost += (int)(config.buildCostMultiplier * level);
        return cost;
    }

    internal static float CalculateBuildTime(BuilderConfig config, int value)
    {
        return config.buildTime + (config.buildTimeMultiplier * value);
    }

    internal static float CalculateHarvestTime(BuilderConfig buildConfig, int level)
    {
        return buildConfig.harvestTime;
    }

    internal static int CalculatePredictIncome(BuilderConfig buildConfig, int level)
    {
        var income = buildConfig.baseIncome;
        income += (int)(buildConfig.growIncome * (level - 1));
        return income;
    }
}