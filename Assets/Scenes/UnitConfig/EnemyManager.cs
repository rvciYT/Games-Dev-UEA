using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private float initialCountdown = 60f;
    [SerializeField] private float waveCooldown = 30f;
    [SerializeField] private float enemySpawnCooldown = 5f;

    private List<Enemy> allEnemies = new List<Enemy>();
    private int score = 0;
    private int wave = 1;
    private bool spawning = false;
    private bool gameStarted = false;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        victoryUI.SetActive(false);
        scoreText.text = "Score: 0";
        waveText.text = "Wave: 0";
        StartCoroutine(CountdownToStart());
    }

    public void ResetManager()
    {
        StopAllCoroutines();
        allEnemies.Clear();
        score = 0;
        wave = 1;
        spawning = false;
        gameStarted = false;
        scoreText.text = "Score: 0";
        waveText.text = "Wave: 0";
        countdownText.gameObject.SetActive(true);
        StartCoroutine(CountdownToStart());
    }

    private IEnumerator CountdownToStart()
    {
        float countdown = initialCountdown;
        while (countdown > 0)
        {
            countdownText.text = $"Time until first wave: {countdown:F0}s";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);
        gameStarted = true;
        waveText.text = $"Wave: {wave}";
        spawnCoroutine = StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        Debug.Log("Starting wave " + wave);
        spawning = true;
        yield return new WaitForSeconds(waveCooldown);

        int enemyCount = Mathf.RoundToInt(5 * Mathf.Pow(1.1f, wave - 1));
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(0);
            yield return new WaitForSeconds(enemySpawnCooldown);
        }

        spawning = false;
        wave++;
        waveText.text = $"Wave: {wave}";
        Debug.Log("Wave " + (wave - 1) + " completed. Starting countdown to next wave.");
        StartCoroutine(CountdownToNextWave());
    }

    private IEnumerator CountdownToNextWave()
    {
        float countdown = waveCooldown;
        while (countdown > 0)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = $"Time until next wave: {countdown:F0}s";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);
        spawnCoroutine = StartCoroutine(SpawnWave());
    }

    private void SpawnEnemy(int index)
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * 35;
        Vector3 spawnPosition = new Vector3(516, 0, 418);
        Enemy enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, transform);
        enemy.gameObject.SetActive(true);
        enemy.GetComponent<Damageable>().onDestroy.AddListener(() => EnemyDestroyed(enemy));
        allEnemies.Add(enemy);
    }

    private void EnemyDestroyed(Enemy enemy)
    {
        allEnemies.Remove(enemy);
        score += 10;
        scoreText.text = $"Score: {score}";
    }
}
