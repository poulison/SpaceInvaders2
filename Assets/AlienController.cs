using UnityEngine;

public class AlienController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 2f;
    public float stepDown = 0.4f;
    public float waitTime = 1f;

    [Header("Tiro")]
    public GameObject alienMissilePrefab;
    public float minFireDelay = 2f;
    public float maxFireDelay = 6f;

    [Header("Pontuação")]
    public int points = 10;

    private Rigidbody2D rb2d;
    private float timer = 0f;
    private float fireTimer = 0f;
    private float nextFire;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = new Vector2(speed, 0);
        nextFire = Random.Range(minFireDelay, maxFireDelay);
    }

    void Update()
    {
        // Troca direção e desce a cada 'waitTime' segundos
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            ChangeDirection();
            timer = 0f;
        }

        // Tiro aleatório
        fireTimer += Time.deltaTime;
        if (fireTimer >= nextFire)
        {
            Fire();
            fireTimer = 0f;
            nextFire = Random.Range(minFireDelay, maxFireDelay);
        }

        // Chegou ao fundo — jogador perde
        if (transform.position.y < -5f)
            GameManager.Instance.GameOver();
    }

    void ChangeDirection()
    {
        var vel = rb2d.linearVelocity;
        vel.x *= -1;
        rb2d.linearVelocity = vel;
        // Desce
        transform.position += Vector3.down * stepDown;
    }

    void Fire()
    {
        if (alienMissilePrefab != null)
            Instantiate(alienMissilePrefab, transform.position, Quaternion.identity);
    }

    public void TakeHit()
    {
        GameManager.Instance.AddScore(points);
        Destroy(gameObject);
    }
}
