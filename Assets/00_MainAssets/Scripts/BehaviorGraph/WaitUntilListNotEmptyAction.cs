using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wait Until List Not Empty", story: "Wait Until [List] Not Empty", category: "Action", id: "ffdb3e908b96ce5ccee26632e65350fb")]
public partial class WaitUntilListNotEmptyAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> List;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (List.Value.Count == 0)
        {
            return Status.Running;
        }
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

