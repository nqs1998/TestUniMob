using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
using System;
public class UpgradeView : MonoBehaviour, IBuilderView
{
    [SerializeField] TextMeshProUGUI txtCoinNeed, txtTitle, txtLevel, txtPredictIncome, txtTimeHarvest;
    [SerializeField] GameObject maxLevelGO;
    [SerializeField] Slider slider;
    [SerializeField] Button btnUpgrade;
    IDisposable disposableLevel, disposableGold;
    BuilderEntity builderEntity;
    public void Setup(BuilderEntity builderEntity)
    {
        BuilderConfig buildConfig = builderEntity.Config;
        this.builderEntity = builderEntity;
        int level = builderEntity.Level.Value;
        if (buildConfig != null)
        {
            bool isMaxLevel = builderEntity.Level.Value >= buildConfig.maxLevel;
            txtCoinNeed.gameObject.SetActive(!isMaxLevel);
            maxLevelGO.SetActive(isMaxLevel);
            txtCoinNeed.text = Calculator.CalculateBuildCost(buildConfig, level).ToString();
            txtTitle.text = buildConfig.builderName;
            txtLevel.text = "Level " + level.ToString();
            txtPredictIncome.text = ((long)builderEntity.ProfitValueRef.Value.GetValue()).FormatNumber();
            txtTimeHarvest.text = Calculator.CalculateHarvestTime(buildConfig, level).ToString();
            slider.value = builderEntity.Level.Value / (float)buildConfig.maxLevel;
            btnUpgrade.interactable = ServiceLocator.Get<EconomySystem>().Gold.Value >= Calculator.CalculateBuildCost(buildConfig, level);
            btnUpgrade.onClick.RemoveAllListeners();
            btnUpgrade.onClick.AddListener(() =>
            {
                builderEntity.OnBuildCommand.Execute(R3.Unit.Default);
            });
        }
    }

    private void OnEnable()
    {
        disposableGold = ServiceLocator.Get<EconomySystem>().Gold.Subscribe(gold =>
        {
            if (builderEntity != null && builderEntity.Config != null)
            {
                btnUpgrade.interactable = gold >= Calculator.CalculateBuildCost(builderEntity.Config, builderEntity.Level.Value);
            }
        }).AddTo(this);
        disposableLevel = builderEntity.Level.Subscribe(_ =>
        {
            Setup(builderEntity);
        }).AddTo(this);
}
    private void OnDisable()
    {
        disposableLevel?.Dispose();
        disposableGold?.Dispose();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}