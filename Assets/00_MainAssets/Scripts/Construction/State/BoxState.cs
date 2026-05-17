using R3;
using System;
using UnityEngine;

public class BoxState : IState
{
    private BuilderEntity builder;
    private IBuilderView builderView;
    IDisposable selectDispoable, buildDispoable;
    public BoxState(BuilderEntity builder)
    {
        this.builder = builder;
        this.builderView = builder.BuildView;
    }

    public void Enter()
    {
        selectDispoable = builder.OnSelectCommand.Subscribe(_ =>
        {
            builderView.Setup(builder);
            builderView.Show();
        }).AddTo(builder);
        buildDispoable = builder.OnBuildCommand.Subscribe(_ =>
        {
            Debug.Log("Build command received in BoxState for " + builder.Config.builderName);
            builder.StateMachine.ChangeState(new BoxOpenState(builder));
        }).AddTo(builder);
    }
   
    public void Update()
    {
        
    }
    public void Exit()
    {
        selectDispoable?.Dispose();
        buildDispoable?.Dispose();
    }
}

