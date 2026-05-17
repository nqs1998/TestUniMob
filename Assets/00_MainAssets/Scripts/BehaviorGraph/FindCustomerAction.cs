using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Customer", story: "Find [Customer] and [TargetPosition]", category: "Action", id: "0578252b55a134aaaba89a6e2a39a00f")]
public partial class FindCustomerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Customer;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var customer = ServiceLocator.Get<CustomerSpawner>().FindCustomerIsWaiting();
        if (customer != null)
        {
            customer.SetVariableValue("isWaiting", false);
            Customer.Value = customer.gameObject;
            TargetPosition.Value = new Vector3(customer.gameObject.transform.position.x, customer.gameObject.transform.position.y, customer.gameObject.transform.position.z - 2);
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

