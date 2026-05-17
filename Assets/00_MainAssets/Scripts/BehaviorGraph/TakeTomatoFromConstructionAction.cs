using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Take Tomatoes From Construction", story: "Take [Tomatoes] From [Construction] to [Points]", category: "Action", id: "8d80a7d3d2ee487e0b079be637d750f1")]
public partial class TakeTomatoFromConstructionAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Tomatoes;
    [SerializeReference] public BlackboardVariable<GameObject> Construction;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Points;

    protected override Status OnStart()
    {
        Tomatoes.Value = Construction.Value.GetComponent<BuilderEntity>().TakeTomatoes(Points.Value);
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

