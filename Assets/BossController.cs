using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 3f;
    public int points = 50;
    private float maxX = 9f;

    void Start()
    {
        // Aparece no canto superior esquerdo
        transform.position = new Vector3(-9f, 5f, 0);
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(speed, 0);
    }

    void Update()
    {
        // Desaparece ao chegar no canto direito
        if (transform.position.x > maxX)
            Destroy(gameObject);
    }

    public void TakeHit()
    {
        GameManager.Instance.AddScore(points);
        Destroy(gameObject);
    }
}
