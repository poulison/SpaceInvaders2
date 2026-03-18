using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI - HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    [Header("Jogo")]
    public int lives = 3;
    public int score = 0;

    [Header("Boss")]
    public GameObject bossPrefab;
    public float minBossDelay = 30f;
    public float maxBossDelay = 50f;

    private int aliensAlive;
    private float bossTimer;
    private float nextBoss;
    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        aliensAlive = FindObjectsByType<AlienController>(FindObjectsSortMode.None).Length;
        nextBoss    = Random.Range(minBossDelay, maxBossDelay);

        UpdateUI();
    }

    void Update()
    {
        if (gameEnded) return;

        bossTimer += Time.deltaTime;
        if (bossTimer >= nextBoss)
        {
            SpawnBoss();
            bossTimer = 0f;
            nextBoss = Random.Range(minBossDelay, maxBossDelay);
        }
    }

    // ─── Pontuação ───────────────────────────────────────────────

    public void AddScore(int pts, bool isAlien = true)
    {
        score += pts;
        UpdateUI();

        if (isAlien)
        {
            aliensAlive--;
            if (aliensAlive <= 0)
                Victory();
        }
    }

    // ─── Vida ────────────────────────────────────────────────────

    public void LoseLife()
    {
        if (gameEnded) return;

        lives--;
        UpdateUI();

        if (lives <= 0)
            GameOver();
    }

    // ─── Fim de Jogo ─────────────────────────────────────────────

    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(IrParaCena("GameOverScene"));
    }

    void Victory()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(IrParaCena("WinScene"));
    }

    IEnumerator IrParaCena(string nomeCena)
    {
        SalvarDados();
        yield return new WaitForSecondsRealtime(0.8f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCena);
    }

    void SalvarDados()
    {
        // Salva o score da partida para exibir na cena de resultado
        PlayerPrefs.SetInt("LastScore", score);

        // Atualiza o High Score se necessário
        int hi = PlayerPrefs.GetInt("HighScore", 0);
        if (score > hi)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        PlayerPrefs.Save();
    }

    // ─── Boss ────────────────────────────────────────────────────

    void SpawnBoss()
    {
        if (bossPrefab != null)
            Instantiate(bossPrefab);
    }

    // ─── UI ──────────────────────────────────────────────────────

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "SCORE\n" + score.ToString("D6");
        if (livesText) livesText.text = "LIVES  " + new string('♥', Mathf.Max(0, lives));
    }
}