using System.Collections;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public static EnemyWave Instance;

    [Header("Wave Settings")]
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public int enemiesPerWave = 5;
    public float waveDelay = 20f;
    public float timeBetweenSpawns = 1f;

    [Header("Wave Progress")]
    public int currentWave = 0;
    public int totalWaves = 5;

    private int aliveEnemies = 0;
    private bool waveInProgress = false;

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
        waveInProgress = true;
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
        
        waveInProgress = false;
    }

    public void OnEnemyKilled()
    {
        aliveEnemies--;
    }
}