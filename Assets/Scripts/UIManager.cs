using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    

    private void Awake()
    {
        /*player.OnDamageTaken += PlayerClass_OnDamageTakenCallback;
        player.OnDeath += PlayerClass_OnDeathCallback;
        player.OnHealthHealed += PlayerClass_OnHealthHealedCallback;*/
    }
    private void OnDestroy()
    {
        /*player.OnDamageTaken -= PlayerClass_OnDamageTakenCallback;
        player.OnDeath -= PlayerClass_OnDeathCallback;
        player.OnHealthHealed -= PlayerClass_OnHealthHealedCallback;*/
    }
    public void PlayerClass_OnDamageTakenCallback(int damageTaken)
    {
        //scoreText.text = player.GetCurrentHealth().ToString();
    }
    public void PlayerClass_OnHealthHealedCallback(int healed)
    {
        //scoreText.text = player.GetCurrentHealth().ToString();
    }
    public void PlayerClass_OnDeathCallback()
    {
        scoreText.text = "Player Dies !";
    }
}