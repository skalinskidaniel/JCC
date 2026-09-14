using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HUDPontuacao : MonoBehaviour
{
       public string prefixo = "Pontos: ";

    private TextMeshProUGUI texto;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        GerenciadorPontuacao.AoPontuarMudar += AtualizarTexto;

        
        if (GerenciadorPontuacao.Instance != null)
        {
            AtualizarTexto(GerenciadorPontuacao.Instance.PontosAtuais);
        }
        else
        {
            AtualizarTexto(0);
        }
    }

    void OnDisable()
    {
        GerenciadorPontuacao.AoPontuarMudar -= AtualizarTexto;
    }

    void AtualizarTexto(int pontos)
    {
        texto.text = prefixo + pontos;
    }
}