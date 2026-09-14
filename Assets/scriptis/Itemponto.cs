using UnityEngine;

/// <summary>
/// Coloque este script em qualquer objeto que deve dar pontos ao ser tocado
/// (moeda, estrela, etc). O Collider do objeto precisa estar marcado como "Is Trigger".
/// </summary>
[RequireComponent(typeof(Collider))]
public class ItemPonto : MonoBehaviour
{
    public int valorPontos = 10;
    public bool destruirAoColetar = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (GerenciadorPontuacao.Instance != null)
        {
            GerenciadorPontuacao.Instance.AdicionarPontos(valorPontos);
        }
        else
        {
            Debug.LogWarning("Nenhum GerenciadorPontuacao encontrado na cena!", this);
        }

        if (destruirAoColetar)
        {
            Destroy(gameObject);
        }
    }
}