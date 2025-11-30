using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SolderUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI nameText;
    public Slider ammoSlider;
    
    private SoldierClass soldier;
    
    void Start()
    {
        soldier = GetComponentInParent<SoldierClass>();
        nameText.text = soldier.soldierName;
    }
    
    void Update()
    {
        // Update ammo text and slider
        ammoText.text = soldier.currentAmmo + "/" + soldier.maxAmmo;
        //ammoSlider.value = (float)soldier.currentAmmo / soldier.maxAmmo;
        
        // Change color when ammo is low
        if (soldier.currentAmmo == 0)
        {
            ammoText.color = Color.red;
        }
        /*else if (soldier.NeedsAmmo)
        {
            ammoText.color = Color.yellow;
        }*/
        else
        {
            ammoText.color = Color.green;
        }
    }
}