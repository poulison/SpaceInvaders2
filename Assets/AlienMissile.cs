using UnityEngine;

public class AlienMissile : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
    }

    void Update()
    {
        if (transform.position.y < -7f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Ignora colisão com aliens, outros mísseis e boss
        if (col.CompareTag("Alien") || col.CompareTag("AlienMissile") || col.CompareTag("Boss"))
            return;

        if (col.CompareTag("Player"))
        {
            GameManager.Instance.LoseLife();
            Destroy(gameObject);
        }
        else if (col.CompareTag("Cover"))
        {
            col.GetComponent<CoverBlock>()?.TakeHit();
            Destroy(gameObject);
        }
    }
}