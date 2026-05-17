using R3;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class UpgradeSO : ScriptableObject, ISaveLoad
{
    public Sprite icon;
    public string upgradeName;
    public string description;
    public long cost;
    public StatModifier statModifier;
    public ReactiveProperty<bool> isUnlock = new();
    private void OnEnable()
    {
        Load();
        isUnlock.Subscribe(_ => Save());
    }

    public void Load()
    {
        isUnlock.Value = PlayerPrefs.GetInt("Upgrade_" + upgradeName, 0) == 1;
        Debug.Log("Upgrade: " + upgradeName + " isUnlock: " + isUnlock.Value);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Upgrade_" + upgradeName, isUnlock.Value ? 1 : 0);
        PlayerPrefs.Save();
    }
}
