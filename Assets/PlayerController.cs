using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;

    [Header("Míssil")]
    public GameObject missilePrefab;
    public float fireCooldown = 0.5f;   // segundos entre cada tiro

    private Rigidbody2D rb2d;
    private float limiteEsquerda;
    private float limiteDireita;
    private float fireTimer = 0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // Calcula os limites da tela automaticamente
        Camera cam = Camera.main;
        limiteEsquerda = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 0.5f;
        limiteDireita  = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 0.5f;
    }

    void Update()
    {
        MoverPlayer();

        fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && fireTimer >= fireCooldown)
        {
            Atirar();
            fireTimer = 0f;
        }
    }

    void MoverPlayer()
    {
        float input = Input.GetAxisRaw("Horizontal"); // -1, 0 ou 1

        Vector2 vel = rb2d.linearVelocity;
        vel.x = input * speed;
        rb2d.linearVelocity = vel;

        // Limita o player dentro da tela
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, limiteEsquerda, limiteDireita);
        transform.position = pos;
    }

    void Atirar()
    {
        if (missilePrefab != null)
            Instantiate(missilePrefab, transform.position, Quaternion.identity);
    }
}