using System.Linq;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{
    public BuilderEntity[] builders;

    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    public BuilderEntity FindBuilderAlready()
    {
        return builders.FirstOrDefault(x => !x.WaitToTake && x.StateMachine.CurrentState is IdleBuilderState && x.IsMaxTomato());
    }
}
