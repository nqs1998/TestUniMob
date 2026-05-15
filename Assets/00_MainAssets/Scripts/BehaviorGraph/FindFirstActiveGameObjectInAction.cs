using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find First Active GameObject in ", story: "Find First Active GameObject in [GameObjectList] and Save to [CurrentTarget]", category: "Action", id: "b84f7a1a2ba5a010661adf273cb372b5")]
public partial class FindFirstActiveGameObjectInAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> GameObjectList;
    [SerializeReference] public BlackboardVariable<GameObject> CurrentTarget;

    protected override Status OnStart()
    {
        foreach (var go in GameObjectList.Value)
        {
            if (go != null && go.activeInHierarchy)
            {
                CurrentTarget.Value = go;
                return Status.Success;
            }
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        foreach (var go in GameObjectList.Value)
        {
            if (go != null && go.activeInHierarchy)
            {
                CurrentTarget.Value = go;
                return Status.Success;
            }
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

