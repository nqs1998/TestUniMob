using R3;
using TMPro;
using UnityEngine;

public class BoxOpenState : IState
{
    private BuilderEntity builder;
    private IBuilderView builderView;
    EconomySystem economySystem;
    TextMeshProUGUI txtTimeCd;
    float timeBuild;
    float cd;
    public BoxOpenState(BuilderEntity builder)
    {
        this.builder = builder;
        this.builderView = builder.BuildView;
    }

    public void Enter()
    {
        economySystem = ServiceLocator.Get<EconomySystem>();
        txtTimeCd = builder.BuildProgressSlider.GetComponentInChildren<TextMeshProUGUI>();
        long cost = Calculator.CalculateBuildCost(builder.Config, builder.Level.Value);
        if (economySystem.TryPurchaseGold(cost, out long remainingGold))
        {
            builder.Level.Value++;
            builder.BuildProgressSlider.gameObject.SetActive(true);
            builder.AnimationBox.Play("BoxOpen");
            builderView.Hide();
            cd = timeBuild = Calculator.CalculateBuildTime(builder.Config, builder.Level.Value);
        }
        else
        {
            builder.StateMachine.ChangeState(new BoxState(builder));
        }
    }

    public void Update()
    {
        if (cd > 0)
        {
            cd -= Time.deltaTime;
            builder.BuildProgressSlider.value = 1 - cd / timeBuild;
            txtTimeCd.text = cd.ToString("F2") + "s";
            if (cd <= 0)
            {
                txtTimeCd.text = "0s";
                builder.EffBuildDone.SetActive(true);
                builder.StateMachine.ChangeState(new IdleBuilderState(builder));
            }
        }

    }
    public void Exit()
    {
        builder.BuildProgressSlider.gameObject.SetActive(false);
        builderView.Hide();
    }
}

