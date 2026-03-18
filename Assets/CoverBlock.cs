using UnityEngine;

public class CoverBlock : MonoBehaviour
{
    [Header("Vida")]
    public int health = 3;             // tiros para destruir o bloco

    // Cores de dano (do mais saudável ao mais destruído)
    private static readonly Color[] damageColors = new Color[]
    {
        new Color(0.18f, 0.83f, 0.18f),  // verde vivo   — health 3
        new Color(0.85f, 0.75f, 0.10f),  // amarelo      — health 2
        new Color(0.85f, 0.35f, 0.10f),  // laranja      — health 1
    };

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        AtualizarCor();
    }

    public void TakeHit()
    {
        health--;

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        AtualizarCor();
    }

    void AtualizarCor()
    {
        if (sr == null) return;
        int idx = Mathf.Clamp(health - 1, 0, damageColors.Length - 1);
        sr.color = damageColors[idx];
    }
}