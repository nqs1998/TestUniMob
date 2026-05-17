using Unity.Behavior;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class RuntimeVariableAssigner : MonoBehaviour
{
    [SerializeField] RuntimeBlackboardAsset globalBlackboardAsset;
    [SerializeField] List<GameObject> PosTargets;
    [SerializeField] GameObject StartPos;
    void Start()
    {
        if (globalBlackboardAsset != null && PosTargets != null)
        {
            AssignReference(StartPos.transform, "StartPos");
            AssignReference(PosTargets, "PosTargets");
        }
    }
    private void AssignReference(object target, string variableName)
    {
        var variable = globalBlackboardAsset.Blackboard.Variables.FirstOrDefault(x => x.Name == variableName);
        if (variable != null) variable.ObjectValue = target;
    }
}
