using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject alienTipo1Prefab;
    public GameObject alienTipo2Prefab;
    public GameObject alienTipo3Prefab;

    [Header("Formação")]
    public int columns = 11;
    public float spacingX = 1.2f;
    public float spacingY = 1.0f;

    [Header("Movimento")]
    public float speed = 2f;
    public float stepDown = 0.4f;

    [Header("Aceleração")]
    public float speedIncreasePerKill = 0.08f;  // quanto acelera por alien morto
    public float maxSpeed = 10f;                 // limite para não ficar impossível

    private float direction = 1f;
    private float limiteEsquerda;
    private float limiteDireita;
    private int totalAliens;
    private float currentSpeed;

    void Start()
    {
        Camera cam = Camera.main;
        limiteEsquerda = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 0.5f;
        limiteDireita  = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 0.5f;

        SpawnFormation();

        totalAliens  = transform.childCount;
        currentSpeed = speed;
    }

    void SpawnFormation()
    {
        Camera cam = Camera.main;
        float topoTela = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y;
        float startY = topoTela - 1.5f;

        GameObject[] prefabPorFileira = {
            alienTipo1Prefab,
            alienTipo2Prefab,
            alienTipo2Prefab,
            alienTipo3Prefab,
            alienTipo3Prefab
        };

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = (col - (columns - 1) / 2f) * spacingX;
                float y = startY - row * spacingY;

                Vector3 pos = new Vector3(x, y, 0);
                GameObject alien = Instantiate(prefabPorFileira[row], pos, Quaternion.identity);
                alien.transform.SetParent(transform);
            }
        }
    }

    void Update()
    {
        // Recalcula velocidade com base nos aliens vivos
        int aliensVivos = transform.childCount;
        int mortos = totalAliens - aliensVivos;
        currentSpeed = Mathf.Min(speed + mortos * speedIncreasePerKill, maxSpeed);

        transform.position += Vector3.right * direction * currentSpeed * Time.deltaTime;

        float maxX = GetMaxX();
        float minX = GetMinX();

        // Sem filhos — nada a fazer
        if (maxX == float.MinValue || minX == float.MaxValue) return;

        if (direction == 1f && maxX >= limiteDireita)
        {
            float overflow = maxX - limiteDireita;
            transform.position -= new Vector3(overflow, 0, 0);
            direction = -1f;
            transform.position += Vector3.down * stepDown;
        }
        else if (direction == -1f && minX <= limiteEsquerda)
        {
            float overflow = limiteEsquerda - minX;
            transform.position += new Vector3(overflow, 0, 0);
            direction = 1f;
            transform.position += Vector3.down * stepDown;
        }
    }

    float GetMaxX()
    {
        float max = float.MinValue;
        foreach (Transform filho in transform)
            if (filho.position.x > max)
                max = filho.position.x;
        return max;
    }

    float GetMinX()
    {
        float min = float.MaxValue;
        foreach (Transform filho in transform)
            if (filho.position.x < min)
                min = filho.position.x;
        return min;
    }
}