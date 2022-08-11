using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnableEnemy[] spawnableEnemys;
    private float chanceToSpawnOneEnemy = 1;

    private float minDelay = 1.5f;
    private float maxDelay = 2f;
    private float delay = 3;
    public int wave;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameUIHandler gameUIHandler;
    [SerializeField] private TutorialManager tutorialManager;

    private bool isSpawning = true;

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameOver || !isSpawning)
        {
            return;
        }

        if(delay <= 0)
        {
            delay = Random.Range(minDelay, maxDelay);
            Spawn();
        }
        else
        {
            delay -= Time.deltaTime;
        }

        int currentScore = gameManager.GetScore();
        if (currentScore >= 0 && currentScore <= 19)
        {
            if (wave != 0)
                ChangeWave(0);
        }
        else if(currentScore >= 20 && currentScore <= 49)
        {
            if (wave != 1)
                ChangeWave(1);
        }
        else if (currentScore >= 50 && currentScore <=69)
        {
            if (wave != 2)
                ChangeWave(2);
        }
        else if (currentScore >= 70 && currentScore <= 99)
        {
            if (wave != 3)
                ChangeWave(3);
        }
        else if (currentScore >= 100 && currentScore <= 119)
        {
            if (wave != 4)
                ChangeWave(4);
        }
        else if (currentScore >= 120 && currentScore <= 149)
        {
            if (wave != 5)
                ChangeWave(5);
        }
        else if (currentScore >= 150 && currentScore <= 169)
        {
            if (wave != 6)
                ChangeWave(6);
        }
        else if (currentScore >= 170 && currentScore <= 199)
        {
            if (wave != 7)
                ChangeWave(7);
        }
        else if (currentScore >= 200 && currentScore <= 219)
        {
            if (wave != 8)
                ChangeWave(8);
        }
        else if (currentScore >= 220 && currentScore <= 249)
        {
            if (wave != 9)
                ChangeWave(9);
        }
        else if (currentScore >= 250 && currentScore <= 269)
        {
            if (wave != 10)
                ChangeWave(10);
        }
        else if (currentScore >= 270 && currentScore <= 299)
        {
            if (wave != 11)
                ChangeWave(11);
        }
        else if (currentScore >= 300)
        {
            if (wave != 12)
                ChangeWave(12);
        }
    }

    private void ChangeWave(int waveNumber)
    {
        gameUIHandler.ShowDifficultyText();
        switch (waveNumber)
        {
            case 0:  // 0
                wave = 0;
                minDelay = 1.5f;
                maxDelay = 2f;
                chanceToSpawnOneEnemy = 1;
                break;

            case 1:  // 20
                wave = 1;
                minDelay = 1.25f;
                maxDelay = 2f;
                chanceToSpawnOneEnemy = 1f;
                break;

            case 2:  // 50
                wave = 2;
                minDelay = 1.25f;
                maxDelay = 1.75f;
                chanceToSpawnOneEnemy = 0.95f;
                break;

            case 3:  // 70
                wave = 3;
                minDelay = 1f;
                maxDelay = 1.75f;
                chanceToSpawnOneEnemy = 0.95f;
                break;

            case 4:  // 100
                wave = 4;
                minDelay = 1f;
                maxDelay = 1.5f;
                chanceToSpawnOneEnemy = 0.925f;
                break;

            case 5:  // 120
                wave = 5;
                minDelay = 0.75f;
                maxDelay = 1.5f;
                chanceToSpawnOneEnemy = 0.925f;
                break;

            case 6:  // 150
                wave = 6;
                minDelay = 0.75f;
                maxDelay = 1.25f;
                chanceToSpawnOneEnemy = 0.9f;
                break;

            case 7:  // 170
                wave = 7;
                minDelay = 0.5f;
                maxDelay = 1.25f;
                chanceToSpawnOneEnemy = 0.9f;
                break;

            case 8:  // 200
                wave = 8;
                minDelay = 0.5f;
                maxDelay = 1f;
                chanceToSpawnOneEnemy = 0.9f;
                break;

            case 9:  // 220
                wave = 9;
                minDelay = 0.5f;
                maxDelay = 0.75f;
                chanceToSpawnOneEnemy = 0.9f;
                break;

            case 10:  // 250
                wave = 10;
                minDelay = 0.5f;
                maxDelay = 0.75f;
                chanceToSpawnOneEnemy = 0.875f;
                break;

            case 11:  // 270
                wave = 11;
                minDelay = 0.25f;
                maxDelay = 0.75f;
                chanceToSpawnOneEnemy = 0.875f;
                break;

            case 12:  // 300
                wave = 12;
                minDelay = 0.25f;
                maxDelay = 0.5f;
                chanceToSpawnOneEnemy = 0.875f;
                break;
        }
    }

    public void AllowSpawing(bool allow)
    {
        isSpawning = allow;
    }

    public bool IsAllowedToSpawn()
    {
        return isSpawning;
    }

    private void Spawn()
    {
        int enemyAmount = 1;

        float enemyAmountPercentage = Random.value;
        if(enemyAmountPercentage > chanceToSpawnOneEnemy)
        {
            enemyAmount = 2;
        }

        Vector2 randomSpawnPos = Vector2.zero;

        for (int x=0; x < enemyAmount; x++)
        {
            float lowBound = 0;
            float highBound = 0;

            float percentage = Random.value;

            for (int i = 0; i < spawnableEnemys.Length; i++)
            {
                lowBound = highBound;
                highBound = lowBound + spawnableEnemys[i].chancesToSpawn[wave];
                if (highBound > 1)
                {
                    Debug.LogError("Error chances to spawn wrong");
                }

                if (lowBound <= percentage && percentage < highBound)
                {
                    randomSpawnPos = GetRandomSpawnPos(randomSpawnPos);
                    var newEnemyObject = Instantiate(spawnableEnemys[i].enemyPrefab, randomSpawnPos, Quaternion.identity);

                    string spawnedEnemyName = spawnableEnemys[i].enemyPrefab.name;

                    if(!SaveSystem.current.AreAllEnemysDiscovered())
                    {
                        tutorialManager.CheckIfEnemyNeedsTutorial(spawnedEnemyName, newEnemyObject);
                    }
                    break;
                }
            }
        }
    }

    private Vector2 GetRandomSpawnPos(Vector2 oldSpawnPos)
    {
        Vector2 newSpawnPosition = new Vector2(0, 0);
        newSpawnPosition.y = 6.5f;
        newSpawnPosition.x = Random.Range(-2.0f, 2.0f);

        if(oldSpawnPos != Vector2.zero)
        {
            while (Vector2.Distance(newSpawnPosition, oldSpawnPos) < 0.75f)
            {
                newSpawnPosition.x = Random.Range(-2.0f, 2.0f); ;
            }
        }

        return newSpawnPosition;
    }
}

[System.Serializable]
public class SpawnableEnemy
{
    public GameObject enemyPrefab;
    public float[] chancesToSpawn;
}
