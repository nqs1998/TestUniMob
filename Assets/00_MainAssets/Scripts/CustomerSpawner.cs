using UnityEngine;
using R3;
using System;
using Unity.Behavior;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private UpgradeSO[] upgrades;
    [SerializeField] private BehaviorGraphAgent customerPrefab;
    [SerializeField] private BehaviorGraphAgent deliveryPrefab;
    [SerializeField] float spawnTime;
    StatProviderRef maxCustomer;
    int currentCustomer;
    [NonSerialized]public List<BehaviorGraphAgent> Customers = new();
    private void Awake()
    {
        ServiceLocator.Register(this);
    }
    void Start()
    {
        maxCustomer = new StatProviderRef();
        maxCustomer.Value = new BaseStat(1);
        Extensions.ApplyStats(maxCustomer, gameObject, upgrades);
        Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ =>
        {
            if (currentCustomer < maxCustomer.Value.GetValue())
            {
                SpawnCustomer();
                currentCustomer++;
            }
        }).AddTo(this);
        for (int i = 0; i < 10; i++)
        {
            Instantiate(deliveryPrefab);
        }
    }
    private void SpawnCustomer()
    {
        Customers.Add(Instantiate(customerPrefab));
    }

    public BehaviorGraphAgent FindCustomerIsWaiting()
    {
        return Customers.Find(x => x.GetVariable("isWaiting", out BlackboardVariable<bool> isWaiting) && isWaiting.Value);
    }
}
