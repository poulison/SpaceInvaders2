using UnityEngine;

public class AlienController : MonoBehaviour
{
    [Header("Tiro")]
    public GameObject alienMissilePrefab;
    public float minFireDelay = 2f;
    public float maxFireDelay = 6f;

    [Header("Pontuação")]
    public int points = 10;

    private float fireTimer = 0f;
    private float nextFire;

    void Start()
    {
        nextFire = Random.Range(minFireDelay, maxFireDelay);
    }

    void Update()
    {
        // ✅ Só cuida do tiro
        fireTimer += Time.deltaTime;
        if (fireTimer >= nextFire)
        {
            Fire();
            fireTimer = 0f;
            nextFire = Random.Range(minFireDelay, maxFireDelay);
        }

        // ✅ Game over se chegar ao fundo
        if (transform.position.y < -5f)
            GameManager.Instance.GameOver();
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