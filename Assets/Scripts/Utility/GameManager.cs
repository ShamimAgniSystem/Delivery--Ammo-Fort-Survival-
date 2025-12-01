using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState = GameState.Playing;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PauseGame()
    {
        gameState = GameState.Pause;
    }
    public void ResumeGame()
    {
        gameState = GameState.Playing;
    }
    public void GameOver()
    {
        Debug.Log("Game Over: ");
    }
    public void WinGame()
    {
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