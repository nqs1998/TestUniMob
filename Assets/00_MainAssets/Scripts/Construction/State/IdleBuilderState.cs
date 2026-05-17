using R3;
using System;
using UnityEngine;

public class IdleBuilderState: IState
{
    private BuilderEntity builder;
    private IBuilderView upgradeView;
    IDisposable selectDispoable, buildDispoable;
    float cd;
    public IdleBuilderState(BuilderEntity builder)
    {
        this.builder = builder;
        this.upgradeView = builder.UpgradeView;
    }
    public void Enter()
    {
        builder.BoxGO.SetActive(false);
        builder.BuilderGO.SetActive(true);
        selectDispoable = builder.OnSelectCommand.Subscribe(_ =>
        {
            upgradeView.Setup(builder);
            upgradeView.Show();
        }).AddTo(builder);
        buildDispoable = builder.OnBuildCommand.Subscribe(_ =>
        {
            if(builder.Level.Value < builder.Config.maxLevel)
            {
                builder.Level.Value++;
            }
        }).AddTo(builder);
    }
    public void Update()
    {
        if(!builder.IsMaxTomato())
        {
            cd += Time.deltaTime;
            if (cd >= Calculator.CalculateHarvestTime(builder.Config, builder.Level.Value))
            {
                cd = 0;
                SpawnTomato();
            }
        }
    }
    
    private void SpawnTomato()
    {
        var tomato = ServiceLocator.Get<TomatoPools>().TomatoPool.Get();
        tomato.transform.position = builder.PlantPoints[builder.PlantPoints.Length - builder.Tomatoes.Count - 1].position;
        builder.Tomatoes.Add(tomato);
    }

    public void Exit()
    {
        selectDispoable?.Dispose();
        buildDispoable?.Dispose();
    }
}

