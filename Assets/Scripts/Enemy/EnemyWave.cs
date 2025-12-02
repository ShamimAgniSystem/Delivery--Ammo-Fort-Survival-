using System.Collections;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public static EnemyWave Instance;

    [Header("Wave Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float waveDelay = 20f;
    [SerializeField] private float timeBetweenSpawns = 1f;

    [Header("Wave Progress")]
    [SerializeField] int currentWave = 0;
    [SerializeField] int totalWaves = 5;

    [SerializeField] private int aliveEnemies = 0;
    [SerializeField] private bool iswaveInProgress = false;
    
    // Events And Delegates...
    public delegate void OnGameCompleted();
    public OnGameCompleted OnGameOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(3f);
        
        while (currentWave < totalWaves)
        {
            currentWave++;
            Debug.Log("Starting Wave " + currentWave);
            yield return StartCoroutine(SpawnWave());
            
            Debug.Log("Wave " + currentWave + " Finished! Waiting " + waveDelay + " seconds...");
            yield return new WaitForSeconds(waveDelay);
        }
    }

    private IEnumerator SpawnWave()
    {
        iswaveInProgress = true;
        int enemiesThisWave = enemiesPerWave + (currentWave * 2);
        
        for (int i = 0; i < enemiesThisWave; i++)
        {
            if (spawnPoints.Length > 0)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                aliveEnemies++;
            }
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        iswaveInProgress = false;
    }
    public void OnEnemyKilled()
    {
        aliveEnemies--;
    }
    #region Getters
    public int GetCurrentAliveEnemies() => aliveEnemies;
    public int GetCurrentWave() => currentWave;
    public bool IsAllWaveFinished()
    {
        return currentWave == totalWaves ? true : false;
    }
    public bool IsWaveAndEnemiesCleared()
    {
        return IsAllWaveFinished() && GetCurrentAliveEnemies() == 0 ? true : false;
    }
    public void IsGameOver()
    {
        if (IsWaveAndEnemiesCleared())
        {
            Debug.Log("Game Over");
            OnGameOver?.Invoke();
        }
    }
    
    #endregion

}