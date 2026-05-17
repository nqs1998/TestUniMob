using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Contruction already", story: "Find [Contruction] already", category: "Action", id: "728e3819c4e99d22fddb0b78f7e2e53c")]
public partial class FindContructionAlreadyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Contruction;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var builder = ServiceLocator.Get<BuilderManager>().FindBuilderAlready();
        if(builder != null)
        {
            Contruction.Value = builder.gameObject;
            builder.WaitToTake = true;
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
    }
}

