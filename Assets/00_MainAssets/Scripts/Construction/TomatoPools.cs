using System;
using UnityEngine;
using UnityEngine.Pool;

public class TomatoPools : MonoBehaviour
{
    public ObjectPool<GameObject> TomatoPool { get; private set; }
    [SerializeField] GameObject tomatoPrefab;
    private void Awake()
    {
        TomatoPool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, defaultCapacity: 3, maxSize: 1000);
        ServiceLocator.Register(this);
    }

    private void OnRelease(GameObject tomato)
    {
        tomato.SetActive(false);
        tomato.transform.SetParent(null);
    }

    private void OnGet(GameObject tomato)
    {
        tomato.SetActive(true);
    }

    private GameObject OnCreate()
    {
        return Instantiate(tomatoPrefab);
    }
}
