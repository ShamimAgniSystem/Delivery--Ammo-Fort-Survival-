using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMainUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Fort fort;
    [SerializeField] private EnemyWave enemyWave;
    
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI playerAmmoText;
    [SerializeField] private TextMeshProUGUI soldierListText;
    [SerializeField] private TextMeshProUGUI fortHealthText;
    
    [Header("Buttons")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        fort.OnFortDamageTaken += FortDamageTakenCallback;
        fort.OnFortDestroy += FortDestroyedCallback;
        enemyWave.OnGameOver += WinGame;
        
        /*restartButton.onClick.AddListener(OnClickReplayButton);
        mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
        resumeButton.onClick.AddListener(OnClickResumeButton);*/
    }

    void Update()
    {
        playerAmmoText.text = "Ammo: " + AmmoSystem.Instance.playerCarry;
        soldierListText.text = GetSoldierStatus();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                OnPause();
            }
            else
            {
                OnClickResumeButton();
            }
        }
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void OnClickReplayButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void OnPause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void OnClickResumeButton()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
    public void WinGame()
    {
        gameWinPanel.SetActive(true);
    }
    
    private void OnDestroy()
    {
        fort.OnFortDamageTaken -= FortDamageTakenCallback;
        fort.OnFortDestroy -= FortDestroyedCallback;
    }
}