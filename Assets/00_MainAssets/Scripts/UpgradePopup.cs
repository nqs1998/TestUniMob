using UnityEngine;
using R3;

public class UpgradePopup : MonoBehaviour
{
    [SerializeField] UpgradeSO[] upgrades;
    [SerializeField] Transform container;
    [SerializeField] UpgradeItemUI itemPrefab;

    private void Awake()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.isUnlock.Value) continue;
            var item = Instantiate(itemPrefab, container);
            item.Setup(upgrade);
            upgrade.isUnlock.Subscribe(isUnlock =>
            {
                if (isUnlock)
                {
                    item.gameObject.SetActive(false);
                }
            }).AddTo(this);
        }
    }
}
