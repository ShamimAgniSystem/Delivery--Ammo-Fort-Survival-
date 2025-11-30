using UnityEngine;
using UnityEngine.UI;

public class SoldierUI : MonoBehaviour
{/*
    [Header("UI Components")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider ammoBar;
    [SerializeField] private Text nameText;
    [SerializeField] private Text statusText;
    [SerializeField] private GameObject lowAmmoWarning;
    [SerializeField] private GameObject outOfAmmoWarning;

    private SoldierClass targetSoldier;

    public void Initialize(SoldierClass soldier)
    {
        targetSoldier = soldier;
        nameText.text = soldier.SoldierData.soldierName;

        // Subscribe to soldier events
        soldier.OnAmmoLow += OnAmmoLow;
        soldier.OnAmmoEmpty += OnAmmoEmpty;

        UpdateUI();
    }

    private void Update()
    {
        if (targetSoldier != null && targetSoldier.IsAlive)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (targetSoldier == null) return;

        // Health bar
        if (healthBar != null)
        {
            //healthBar.value = (float)targetSoldier.GetCurrentHealth() / 100f;
        }

        // Ammo bar
        if (ammoBar != null)
        {
            ammoBar.value = (float)targetSoldier.SoldierData.currentAmmo / targetSoldier.SoldierData.maxAmmo;
        }

        // Status text
        if (statusText != null)
        {
            statusText.text = targetSoldier.CurrentState.ToString();
            statusText.color = GetStateColor(targetSoldier.CurrentState);
        }
    }

    private void OnAmmoLow(SoldierClass soldier)
    {
        if (lowAmmoWarning != null)
            lowAmmoWarning.SetActive(true);
    }

    private void OnAmmoEmpty(SoldierClass soldier)
    {
        if (outOfAmmoWarning != null)
            outOfAmmoWarning.SetActive(true);

        if (lowAmmoWarning != null)
            lowAmmoWarning.SetActive(false);
    }

    private Color GetStateColor(SoldierState state)
    {
        switch (state)
        {
            case SoldierState.Patrol: return Color.green;
            case SoldierState.Combat: return Color.red;
            case SoldierState.Reloading: return Color.yellow;
            case SoldierState.Dead: return Color.gray;
            default: return Color.white;
        }
    }

    private void OnDestroy()
    {
        if (targetSoldier != null)
        {
            targetSoldier.OnAmmoLow -= OnAmmoLow;
            targetSoldier.OnAmmoEmpty -= OnAmmoEmpty;
        }
    }*/
}