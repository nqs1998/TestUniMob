using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Custom Navmesh GameObject to Target", story: "Custom Navmesh [Agent] to [Target]", category: "Action", id: "40c28bfdb8f67945f9a6bb46cd0269ca")]
public partial class CustomNavmeshGameObjectToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    NavMeshAgent agent;
    protected override Status OnStart()
    {
        agent = Agent.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(Target.Value.transform.position);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

