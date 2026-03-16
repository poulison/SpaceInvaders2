using UnityEngine;

public class PlayerMissile : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        // Remove se sair da tela
        if (transform.position.y > 7f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Alien") || col.CompareTag("Boss"))
        {
            col.GetComponent<AlienController>()?.TakeHit();
            Destroy(gameObject);
        }
    }
}
