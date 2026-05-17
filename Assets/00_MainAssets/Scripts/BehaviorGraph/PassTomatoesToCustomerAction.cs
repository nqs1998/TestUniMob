using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using DG.Tweening;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Pass Tomatoes to customer", story: "Pass [Tomatoes] to [Customers] take by [Construction] and show [Particles]", category: "Action", id: "915202addb79964a686c40e98aa0eba6")]
public partial class PassTomatoesToCustomerAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Tomatoes;
    [SerializeReference] public BlackboardVariable<GameObject> Customers;
    [SerializeReference] public BlackboardVariable<GameObject> Construction;
    [SerializeReference] public BlackboardVariable<GameObject> Particles; 
    protected override Status OnStart()
    {
        var customerGraph = Customers.Value.GetComponent<BehaviorGraphAgent>();
        customerGraph.SetVariableValue("Tomatoes", Tomatoes.Value);
        customerGraph.GetVariable("ItemPoints",out BlackboardVariable<List<GameObject>> itemPoints);
        for (int i = 0; i < Tomatoes.Value.Count; i++)
        {
            var tomato = Tomatoes.Value[i];
            var index = i;
            tomato.transform.DOMove(itemPoints.Value[i].transform.position, 0.5f).SetDelay(0.25f * i).OnComplete(() =>
            {
                tomato.transform.SetParent(itemPoints.Value[index].transform);
            });
        }
        var gold = Construction.Value.GetComponent<BuilderEntity>().ProfitValueRef.Value.GetValue();
        ServiceLocator.Get<EconomySystem>().Gold.Value += (long)gold;
        GameObject.Instantiate(Particles.Value, Customers.Value.transform.position + Vector3.up * 2, Quaternion.identity);
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

