using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Jogo")]
    public int lives = 3;
    public int score = 0;
    public GameObject bossPrefab;
    public float minBossDelay = 30f;
    public float maxBossDelay = 50f;

    private int aliensAlive;
    private float bossTimer;
    private float nextBoss;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        aliensAlive = FindObjectsOfType<AlienController>().Length;
        nextBoss = Random.Range(minBossDelay, maxBossDelay);
        UpdateUI();
    }

    void Update()
    {
        // Spawn da nave chefe
        bossTimer += Time.deltaTime;
        if (bossTimer >= nextBoss)
        {
            SpawnBoss();
            bossTimer = 0f;
            nextBoss = Random.Range(minBossDelay, maxBossDelay);
        }
    }

    public void AddScore(int pts)
    {
        score += pts;
        UpdateUI();
        aliensAlive--;
        if (aliensAlive <= 0)
            Victory();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();
        if (lives <= 0)
            GameOver();
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    void Victory()
    {
        Time.timeScale = 0;
        if (victoryPanel) victoryPanel.SetActive(true);
    }

    void SpawnBoss()
    {
        if (bossPrefab != null)
            Instantiate(bossPrefab);
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "SCORE: " + score;
        if (livesText) livesText.text = "LIVES: " + lives;
    }
}
