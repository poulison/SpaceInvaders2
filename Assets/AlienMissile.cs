using UnityEngine;

public class AlienMissile : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        // Garante que o míssil vai para baixo desde o início
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
    }

    void Update()
    {
        // Destrói ao sair da tela pelo fundo
        if (transform.position.y < -7f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Ignora colisão com aliens e outros mísseis
        if (col.CompareTag("Alien") || col.CompareTag("AlienMissile") || col.CompareTag("Boss"))
            return;

        // Acerta o player
        if (col.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife();
            Destroy(gameObject);
        }
    }
}