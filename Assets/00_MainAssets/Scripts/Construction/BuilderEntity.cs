using DG.Tweening;
using R3;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class BuilderEntity : MonoBehaviour, ISelectable, ISaveLoad
{
    [SerializeField] string guid = Guid.NewGuid().ToString();
    [field: SerializeField] public BuilderConfig Config { get; private set; }
    [field: SerializeField] public GameObject BoxGO { get; private set; }
    [field: SerializeField] public GameObject BuilderGO { get; private set; }
    [field: SerializeField] public Slider BuildProgressSlider { get; private set; }
    [field: SerializeField] public Animation AnimationBox { get; private set; }
    [field: SerializeField] public BuilderView BuildView { get; private set; }
    [field: SerializeField] public UpgradeView UpgradeView { get; private set; }
    [field: SerializeField] public GameObject EffBuildDone { get; private set; }
    [field: SerializeField] public Transform[] PlantPoints { get; private set; }
    [field: SerializeField] public UpgradeSO[] Upgrades { get; private set; }

    // Runtime
    public SerializableReactiveProperty<int> Level { get; private set; } = new();
    public StateMachine StateMachine { get; private set; } = new();
    public ReactiveCommand OnSelectCommand { get; private set; } = new();
    public ReactiveCommand OnBuildCommand { get; private set; } = new();
    public ObservableList<GameObject> Tomatoes { get; private set; } = new();
    public bool WaitToTake { get; set; }

    public StatProviderRef ProfitValueRef { get; private set; } = new();
    private void Awake()
    {
        Load();
        Level.Subscribe(_ =>
        {
            ApplyProfitStat();
            Save();
        }).AddTo(this);
        ApplyProfitStat();
    }

    private void ApplyProfitStat()
    {
        ProfitValueRef = new StatProviderRef();
        ProfitValueRef.Value = new BaseStat(Calculator.CalculatePredictIncome(Config, Level.Value));
        Extensions.ApplyStats(ProfitValueRef, this.gameObject, Upgrades);
    }

    public void Start()
    {
        if (Level.Value == 0)
        {
            StateMachine.ChangeState(new BoxState(this));
        }
        else
        {
            StateMachine.ChangeState(new IdleBuilderState(this));
        }
    }
    private void Update()
    {
        StateMachine.Update();
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public void Select()
    {
        var state = StateMachine.CurrentState;
        if (state.GetType() == typeof(BoxState) || state.GetType() == typeof(IdleBuilderState))
        {
            OnSelectCommand.Execute(Unit.Default);
        }
    }
    public void Deselect()
    {
        BuildView.Hide();
        UpgradeView.Hide();
    }

    public void Save()
    {
        PlayerPrefs.SetString($"Builder_{guid}", Level.Value.ToString());
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey($"Builder_{guid}"))
        {
            Level.Value = int.Parse(PlayerPrefs.GetString($"Builder_{guid}"));
        }
        else
        {
            Save();
        }
    }
    [ContextMenu("Reset GUID")]
    public void ResetGUID()
    {
        guid = Guid.NewGuid().ToString();
    }
    public bool IsMaxTomato()
    {
        return Tomatoes.Count >= PlantPoints.Length;
    }
    public List<GameObject> TakeTomatoes(List<GameObject> points)
    {
        WaitToTake = false;
        var takenTomatoes = new List<GameObject>();
        for (int i = 0; i < Tomatoes.Count; i++)
        {
            var tomato = Tomatoes[i];
            var index = i;
            tomato.transform.DOMove(points[i].transform.position, 0.5f).SetDelay(0.25f * i).OnComplete(() =>
            {
                tomato.transform.SetParent(points[index].transform);
            });
            takenTomatoes.Add(tomato);
        }
        Tomatoes.Clear();
        return takenTomatoes;
    }
}
