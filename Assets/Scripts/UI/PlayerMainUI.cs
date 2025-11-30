using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMainUI : MonoBehaviour
{
    public TextMeshProUGUI playerAmmoText;
    public TextMeshProUGUI soldierListText;
    
    void Update()
    {
        // Show player's carried ammo
        playerAmmoText.text = "Ammo: " + AmmoSystem.Instance.playerCarry;
        
        // Show soldier status list
        soldierListText.text = GetSoldierStatus();
    }
    
    string GetSoldierStatus()
    {
        string status = "Soldiers:\n";
        
        foreach (SoldierClass soldier in SoldierManager.Instance.AllSoldiers)
        {
            if (soldier.isAlive)
            {
                status += soldier.soldierName + ": " + soldier.currentAmmo + " ammo\n";
            }
            else
            {
                status += soldier.soldierName + ": DEAD\n";
            }
        }
        
        return status;
    }
}