using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI - Textos")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;

    [Header("UI - Painéis")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("UI - Score Final nos Painéis")]
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI victoryScoreText;
    public TextMeshProUGUI newRecordText;       // Texto "NOVO RECORDE!" (opcional)

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

        // Conta apenas aliens normais (não o boss)
        aliensAlive = FindObjectsByType<AlienController>(FindObjectsSortMode.None).Length;

        nextBoss = Random.Range(minBossDelay, maxBossDelay);

        // Garante que os painéis começam escondidos
        if (gameOverPanel)  gameOverPanel.SetActive(false);
        if (victoryPanel)   victoryPanel.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        if (gameEnded) return;

        // Spawn do Boss
        bossTimer += Time.deltaTime;
        if (bossTimer >= nextBoss)
        {
            SpawnBoss();
            bossTimer = 0f;
            nextBoss = Random.Range(minBossDelay, maxBossDelay);
        }
    }

    // ─── Pontuação ──────────────────────────────────────────────

    /// <param name="isAlien">false para o Boss (não conta na vitória)</param>
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
        StartCoroutine(MostrarPainel(gameOverPanel));
    }

    void Victory()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(MostrarPainel(victoryPanel));
    }

    IEnumerator MostrarPainel(GameObject painel)
    {
        bool novoRecorde = SalvarHighScore();

        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0f;

        if (painel) painel.SetActive(true);

        // Preenche score final no painel correto
        string scoreStr = "SCORE: " + score.ToString("D6");
        if (painel == gameOverPanel && gameOverScoreText)
            gameOverScoreText.text = scoreStr;
        if (painel == victoryPanel && victoryScoreText)
            victoryScoreText.text = scoreStr;

        // Exibe "NOVO RECORDE!" se aplicável
        if (newRecordText)
            newRecordText.gameObject.SetActive(novoRecorde);
    }

    bool SalvarHighScore()
    {
        int hi = PlayerPrefs.GetInt("HighScore", 0);
        if (score > hi)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    // ─── Boss ────────────────────────────────────────────────────

    void SpawnBoss()
    {
        if (bossPrefab != null)
            Instantiate(bossPrefab);
    }

    // ─── Botões de UI ────────────────────────────────────────────

    /// <summary>Conecte ao botão "Reiniciar" no painel de Game Over / Vitória.</summary>
    public void Reiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Conecte ao botão "Menu Principal".</summary>
    public void IrParaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    // ─── UI ──────────────────────────────────────────────────────

    void UpdateUI()
    {
        if (scoreText) scoreText.text = "SCORE\n" + score.ToString("D6");
        if (livesText) livesText.text = "LIVES  " + new string('♥', Mathf.Max(0, lives));
    }
}