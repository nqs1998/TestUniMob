using TMPro;
using UnityEngine;
using UnityEngine.UI;
using R3;
public class BuilderView : MonoBehaviour, IBuilderView
{
    [SerializeField] Image iconResource;
    [SerializeField] TextMeshProUGUI txtCoinNeed, txtTitle;
    [SerializeField] Button btnBuild;
    public void Setup(BuilderEntity builderEntity)
    {
        BuilderConfig buildConfig = builderEntity.Config;
        int level = builderEntity.Level.Value;
        if (buildConfig != null)
        {
            txtCoinNeed.text = ((long)Calculator.CalculateBuildCost(buildConfig, level)).FormatNumber();
            txtTitle.text = buildConfig.builderName;
            iconResource.sprite = buildConfig.icon;
            ServiceLocator.Get<EconomySystem>().Gold.Subscribe(gold =>
            {
                btnBuild.interactable = gold >= Calculator.CalculateBuildCost(buildConfig, level);
            }).AddTo(this);
            btnBuild.interactable = ServiceLocator.Get<EconomySystem>().Gold.Value >= Calculator.CalculateBuildCost(buildConfig, level);
            btnBuild.onClick.RemoveAllListeners();
            btnBuild.onClick.AddListener(() => 
            {
                Debug.Log("Build button clicked for " + buildConfig.builderName);
                builderEntity.OnBuildCommand.Execute(R3.Unit.Default);
            });
        }
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
