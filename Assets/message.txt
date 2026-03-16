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

    private float direction = 1f;
    private float limiteEsquerda;
    private float limiteDireita;

    void Start()
    {
        Camera cam = Camera.main;

        // ✅ Limites da tela
        limiteEsquerda = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 0.5f;
        limiteDireita  = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 0.5f;

        SpawnFormation();
    }

    void SpawnFormation()
    {
        Camera cam = Camera.main;
        float topoTela = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0)).y;
        float startY = topoTela - 1.5f;

        GameObject[] prefabPorFileira = {
            alienTipo1Prefab, // fileira 0 - topo   (30 pts)
            alienTipo2Prefab, // fileira 1           (20 pts)
            alienTipo2Prefab, // fileira 2           (20 pts)
            alienTipo3Prefab, // fileira 3           (10 pts)
            alienTipo3Prefab  // fileira 4 - baixo   (10 pts)
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
        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        float maxX = GetMaxX();
        float minX = GetMinX();

        if (direction == 1f && maxX >= limiteDireita)
        {
            // ✅ Corrige o excesso e inverte
            float overflow = maxX - limiteDireita;
            transform.position -= new Vector3(overflow, 0, 0);
            direction = -1f;
            transform.position += Vector3.down * stepDown;
        }
        else if (direction == -1f && minX <= limiteEsquerda)
        {
            // ✅ Corrige o excesso e inverte
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