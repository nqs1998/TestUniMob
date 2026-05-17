using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Release Tomato", story: "Release [Tomatoes]", category: "Action", id: "6788989fb500b333174aa1c6b0ea56c5")]
public partial class ReleaseTomatoAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Tomatoes;

    protected override Status OnStart()
    {
        foreach (var item in Tomatoes.Value)
        {
            ServiceLocator.Get<TomatoPools>().TomatoPool.Release(item);
        }
        Tomatoes.Value.Clear();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

