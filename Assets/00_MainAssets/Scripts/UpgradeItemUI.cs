using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtName, txtDescription,txtGold;
    [SerializeField] Image icon;
    [SerializeField] Button btnUpgrade;
    public void Setup(UpgradeSO upgradeSO)
    {
        if (upgradeSO.isUnlock.Value)
        {
            // Already unlocked
            gameObject.SetActive(false);
            return;
        }
        txtName.text = upgradeSO.upgradeName;
        txtDescription.text = upgradeSO.description;
        txtGold.text = upgradeSO.cost.FormatNumber();
        icon.sprite = upgradeSO.icon;
        btnUpgrade.onClick.RemoveAllListeners();
        btnUpgrade.onClick.AddListener(() =>
        {       
            var economySystem = ServiceLocator.Get<EconomySystem>();
            // Check if player has enough gold
            if (economySystem.TryPurchaseGold(upgradeSO.cost, out long remainingGold))
            {
                upgradeSO.isUnlock.Value = true;
            }
        });
        gameObject.SetActive(true);
    }
}   