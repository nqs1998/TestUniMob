using Unity.Behavior;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class RuntimeVariableAssigner : MonoBehaviour
{
    [SerializeField] RuntimeBlackboardAsset globalBlackboardAsset;
    [SerializeField] List<GameObject> PosTargets;
    void Start()
    {
        if (globalBlackboardAsset != null && PosTargets != null)
        {
            var variable = globalBlackboardAsset.Blackboard.Variables.FirstOrDefault(x => x.Name == "PosTargets");
            if(variable != null) variable.ObjectValue = PosTargets;
        }
    }
}
