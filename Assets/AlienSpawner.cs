using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public GameObject alienTipo1Prefab;  // 30 pts - fileira do topo
    public GameObject alienTipo2Prefab;  // 20 pts - fileiras do meio
    public GameObject alienTipo3Prefab;  // 10 pts - fileiras de baixo

    public int columns = 11;
    public float spacingX = 1.2f;
    public float spacingY = 1.0f;

    void Start()
    {
        SpawnFormation();
    }

    void SpawnFormation()
    {
        // Define qual prefab usa em cada fileira (0 = topo, 4 = baixo)
        GameObject[] prefabPorFileira = {
            alienTipo1Prefab,  // fileira 0 - topo
            alienTipo2Prefab,  // fileira 1
            alienTipo2Prefab,  // fileira 2
            alienTipo3Prefab,  // fileira 3
            alienTipo3Prefab   // fileira 4 - baixo
        };

        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = (col - (columns - 1) / 2f) * spacingX;
                float y = -row * spacingY;

                Vector3 pos = transform.position + new Vector3(x, y, 0);
                GameObject alien = Instantiate(prefabPorFileira[row], pos, Quaternion.identity);
                alien.transform.SetParent(transform);
            }
        }
    }
}