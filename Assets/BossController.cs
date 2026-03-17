using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 3f;

    [Header("Vida")]
    public int maxHealth = 4;
    private int currentHealth;

    [Header("Feedback visual")]
    public SpriteRenderer spriteRenderer;  // opcional — o script busca automaticamente
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;

    [Header("Pontuação")]
    public int points = 150;

    private float limiteEsquerda;
    private float limiteDireita;
    private float direction = 1f;
    private Rigidbody2D rb;
    private Color originalColor;
    private Coroutine flashCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        Camera cam = Camera.main;
        limiteEsquerda = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 1f;
        limiteDireita  = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 1f;

        float spawnY = GetTopAlienY(cam);

        direction = Random.value > 0.5f ? 1f : -1f;
        float startX = direction == 1f ? limiteEsquerda : limiteDireita;

        transform.position = new Vector3(startX, spawnY, 0);
        rb.linearVelocity = new Vector2(speed * direction, 0);
    }

    void Update()
    {
        if (direction == 1f  && transform.position.x > limiteDireita)
            Destroy(gameObject);
        else if (direction == -1f && transform.position.x < limiteEsquerda)
            Destroy(gameObject);
    }

    public void TakeHit()
    {
        currentHealth--;

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            GameManager.Instance.AddScore(points, isAlien: false);
            Destroy(gameObject);
        }
    }

    IEnumerator FlashRed()
    {
        if (spriteRenderer != null) spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        if (spriteRenderer != null) spriteRenderer.color = originalColor;
    }

    float GetTopAlienY(Camera cam)
    {
        AlienController[] aliens = FindObjectsByType<AlienController>(FindObjectsSortMode.None);

        if (aliens.Length == 0)
            return cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y - 0.8f;

        float maxY = float.MinValue;
        foreach (var alien in aliens)
            if (alien.transform.position.y > maxY)
                maxY = alien.transform.position.y;

        return maxY + 1.2f;
    }
}