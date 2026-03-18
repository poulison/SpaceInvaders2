using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOverSceneManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;       // "SCORE: 000000"
    public TextMeshProUGUI highScoreText;   // "HI-SCORE: 000000"
    public TextMeshProUGUI newRecordText;   // "NOVO RECORDE!" — deixe desativado no editor

    [Header("Animação")]
    public float blinkInterval = 0.6f;     // pisca no texto de novo recorde

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int hiScore   = PlayerPrefs.GetInt("HighScore", 0);

        if (scoreText)
            scoreText.text = "SCORE: " + lastScore.ToString("D6");

        if (highScoreText)
            highScoreText.text = "HI-SCORE: " + hiScore.ToString("D6");

        // Exibe "NOVO RECORDE!" apenas se o último score é igual ao high score
        bool novoRecorde = lastScore > 0 && lastScore == hiScore;
        if (newRecordText)
        {
            newRecordText.gameObject.SetActive(novoRecorde);
            if (novoRecorde)
                StartCoroutine(PiscarTexto(newRecordText));
        }
    }

    // ─── Botões ──────────────────────────────────────────────────

    /// <summary>Conecte ao botão TENTAR NOVAMENTE.</summary>
    public void Reiniciar()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>Conecte ao botão MENU.</summary>
    public void IrParaMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // ─── Animação ────────────────────────────────────────────────

    IEnumerator PiscarTexto(TextMeshProUGUI texto)
    {
        while (true)
        {
            texto.enabled = !texto.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}