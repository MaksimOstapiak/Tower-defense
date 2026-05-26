using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemySpawnData
{
    public string enemyName;
    public GameObject enemyPrefab;
    public int cost;
}

public enum GameState 
{ 
    Preparation, 
    Battle, 
    RoundEnd 
}

public class WaveManager : MonoBehaviour
{
     public int currentRound = 1;
    [Header("Налаштування раунду")]
    public int currentAttackBudget = 200;
    public int budgetIncreasePerRound = 150;
    public int maxEnemiesPerWave = 50;
    public float minSpawnInterval = 0.4f;
    public float maxSpawnInterval = 1.0f;
    
    [Header("Налаштування Боса (Dark Knight)")]
    public EnemySpawnData darkKnightData;
    public int bossWaveInterval = 5;

    public GameState currentState;

    [Header("Посилання на об'єкти сцени")]
    public Transform spawnPoint;
    public Transform[] pathWaypoints;
    public List<EnemySpawnData> availableEnemies;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI gameStateText;

    private Queue<GameObject> currentWave = new Queue<GameObject>();

    void Start()
    {
        StartPreparationPhase();
    }

    public void StartPreparationPhase()
    {
        currentState = GameState.Preparation;
        
        if (waveText != null)
        {
            waveText.text = "Wawe: " + currentRound;
            
        }
        if (gameStateText != null)
        {
            gameStateText.text = "Stage: Preparation";
        }
        GenerateWave();
    }

    private void GenerateWave()
    {
        currentWave.Clear();
        int remainingBudget = currentAttackBudget;
        int enemiesAdded = 0;

        bool isBossWave = (currentRound % bossWaveInterval == 0);

        if (isBossWave && darkKnightData.enemyPrefab != null)
        {
            int countBosses = currentRound / bossWaveInterval;
            for(int i = 0; i < countBosses && remainingBudget > darkKnightData.cost; i++){
            remainingBudget -= darkKnightData.cost;
            currentWave.Enqueue(darkKnightData.enemyPrefab);
            enemiesAdded++;
            }
        }

        while (remainingBudget > 0 && enemiesAdded < maxEnemiesPerWave)
        {
            EnemySpawnData randomEnemy = availableEnemies[Random.Range(0, availableEnemies.Count)];

            if (remainingBudget >= randomEnemy.cost)
            {
                currentWave.Enqueue(randomEnemy.enemyPrefab);
                remainingBudget -= randomEnemy.cost;
                enemiesAdded++;
            }
            else
            {
                bool canAffordAny = false;
                foreach(var enemy in availableEnemies)
                {
                    if(remainingBudget >= enemy.cost)
                    {
                        canAffordAny = true;
                        break;
                    }
                }
                
                if (!canAffordAny) break; 
            }
        }
        
    }

    public void StartBattlePhase()
    {
        if (currentState != GameState.Preparation) return;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.roundStartSound);

        currentState = GameState.Battle;
        if (gameStateText != null)
        {
            gameStateText.text = "Stage: FIGHT!";
        }
        StartCoroutine(SpawnWaveCoroutine());
    }

    private IEnumerator SpawnWaveCoroutine()
    {
        while (currentWave.Count > 0)
        {
            GameObject enemyToSpawn = currentWave.Dequeue();

            GameObject newEnemy = ObjectPooler.Instance.GetObject(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            
            EnemyMovement movementScript = newEnemy.GetComponent<EnemyMovement>();
            if (movementScript != null)
            {
                movementScript.waypoints = pathWaypoints;
            }

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }

        StartCoroutine(WaitForWaveEndCoroutine());
    }

    private IEnumerator WaitForWaveEndCoroutine()
    {

        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return new WaitForSeconds(1f);
        }

        EndRound();
    }

    private void EndRound()
    {
        currentState = GameState.RoundEnd;

        currentAttackBudget += budgetIncreasePerRound; 
        currentRound++;
        if (gameStateText != null)
        {
            gameStateText.text = "Stage: break";
        }

        StartPreparationPhase();
    }
}