using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class WinSceneManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;       // "SCORE: 000000"
    public TextMeshProUGUI highScoreText;   // "HI-SCORE: 000000"
    public TextMeshProUGUI newRecordText;   // "NOVO RECORDE!" — deixe desativado no editor

    [Header("Animação")]
    public float blinkInterval = 0.5f;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        int hiScore   = PlayerPrefs.GetInt("HighScore", 0);

        if (scoreText)
            scoreText.text = "SCORE: " + lastScore.ToString("D6");

        if (highScoreText)
            highScoreText.text = "HI-SCORE: " + hiScore.ToString("D6");

        bool novoRecorde = lastScore > 0 && lastScore == hiScore;
        if (newRecordText)
        {
            newRecordText.gameObject.SetActive(novoRecorde);
            if (novoRecorde)
                StartCoroutine(PiscarTexto(newRecordText));
        }
    }

    // ─── Botões ──────────────────────────────────────────────────

    /// <summary>Conecte ao botão JOGAR NOVAMENTE.</summary>
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