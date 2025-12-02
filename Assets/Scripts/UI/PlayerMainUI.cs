using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerAmmoText;
    [SerializeField] private TextMeshProUGUI soldierListText;
    [SerializeField] private TextMeshProUGUI fortHealthText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    
    [Header("References")]
    [SerializeField] private Fort fort;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        fort.OnFortDamageTaken += FortDamageTakenCallback;
        fort.OnFortDestroy += FortDestroyedCallback;
        
        restartButton.onClick.AddListener(OnClickReplayButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
    }

    void Update()
    {
        playerAmmoText.text = "Ammo: " + AmmoSystem.Instance.playerCarry;
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

    private void FortDamageTakenCallback()
    {
        fortHealthText.text = "Fort-Health: " + fort.Health;
    }

    private void FortDestroyedCallback()
    {
        gameOverPanel.SetActive(true);
    }

    public void OnClickMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickReplayButton()
    {
        SceneManager.LoadScene(1);
    }
}