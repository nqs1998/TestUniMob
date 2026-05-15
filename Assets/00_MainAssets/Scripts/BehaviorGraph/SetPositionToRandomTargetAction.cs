using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Position To Random Target", story: "Set [Transform] position to [Targets]", category: "Action", id: "863a8f97216cf60e3720663afa153eaf")]
public partial class SetPositionToRandomTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Transform;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Targets;
    private Vector3 m_Destination;
    private GameObject m_Target;
    protected override Status OnStart()
    {
        if(Transform.Value == null || Targets.Value == null || Targets.Value.Count == 0)
        {
            LogFailure("No Transform or Target set.");
            return Status.Failure;
        }
        m_Target = Targets.Value[UnityEngine.Random.Range(0, Targets.Value.Count)];
        m_Destination = m_Target.transform.position;
        Transform.Value.transform.position = m_Destination;
        return Status.Success;
    }
}

