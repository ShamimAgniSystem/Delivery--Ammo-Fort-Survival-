using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settinsButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private SettingsPanelHandler settingsPanelHandler;

    private void Start()
    {
        playButton.onClick.AddListener(OnClickPlayButton); 
        settinsButton.onClick.AddListener(OnClickSettingsButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
    }
    #region Methods

    private void OnClickPlayButton()
    {
        SceneManager.LoadScene(1);
        GameManager.Instance.gameState = GameState.Playing;
    }
    private void OnClickSettingsButton()
    {
        settingsPanelHandler.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnClickQuitButton()
    {
        Application.Quit();
    }
    #endregion
}
 