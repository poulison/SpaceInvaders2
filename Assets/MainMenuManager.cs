using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
 
public class MainMenuManager : MonoBehaviour
{
    [Header("Background")]
    // Arraste aqui o componente Image do seu Canvas que vai receber a imagem de fundo
    public Image backgroundImage;
    // Arraste aqui a Sprite importada da imagem de fundo
    public Sprite backgroundSprite;
 
    [Header("UI - Textos")]
    public TextMeshProUGUI highScoreText;   // Texto "High Score: 0"
 
 
 
    void Start()
    {
        // ── Background ──────────────────────────────────────────
        if (backgroundImage != null && backgroundSprite != null)
        {
            backgroundImage.sprite = backgroundSprite;
            backgroundImage.preserveAspect = false; // estica para preencher a tela
        }
 
        // ── High Score ──────────────────────────────────────────
        int hi = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText)
            highScoreText.text = "High Score: " + hi;
 
    }
 
    // ─── Botões ──────────────────────────────────────────────────
 
    /// <summary>Conecte ao botão PLAY.</summary>
    public void Jogar()
    {
        SceneManager.LoadScene("GameScene");
    }
 
    /// <summary>Conecte ao botão de som (toggle mute).</summary>
    public void ToggleSom()
    {
        AudioListener.pause = !AudioListener.pause;
    }
 
    /// <summary>Conecte ao botão SAIR (se houver).</summary>
    public void Sair()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
 
 
}