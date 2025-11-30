using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxWaves = 5;
    public bool gameRunning = true;
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver(string reason)
    {
        gameRunning = false;
        Debug.Log("Game Over: " + reason);
    }

    public void WinGame()
    {
        gameRunning = false;
        Debug.Log("You Win! All zombies defeated!");
    }
}

public enum GameState
{
    Pause,
    Playing,
    WinGame,
    GameOver
}