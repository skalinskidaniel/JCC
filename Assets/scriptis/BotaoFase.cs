using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Coloque este script em CADA bot\u00e3o de fase do menu (um componente por bot\u00e3o).
/// Ele verifica automaticamente se a fase j\u00e1 foi desbloqueada e, se n\u00e3o,
/// desativa o bot\u00e3o e mostra um \u00edcone de cadeado (opcional).
/// </summary>
[RequireComponent(typeof(Button))]
public class BotaoFase : MonoBehaviour
{
    
    public string nomeDaCena;

    
    public int numeroFase = 1;

    
    public GameObject iconeCadeado;

    private Button botao;

    void Start()
    {
        botao = GetComponent<Button>();
        AtualizarEstadoDoBotao();
    }

    void AtualizarEstadoDoBotao()
    {
        bool desbloqueada = ProgressoFases.FaseEstaDesbloqueada(numeroFase);
        botao.interactable = desbloqueada;

        if (iconeCadeado != null)
        {
            iconeCadeado.SetActive(!desbloqueada);
        }
    }

    
    public void CarregarFase()
    {
        if (!ProgressoFases.FaseEstaDesbloqueada(numeroFase))
        {
            Debug.Log($"Fase {numeroFase} ainda est\u00e1 bloqueada!");
            return;
        }

        SceneManager.LoadScene(nomeDaCena);
    }
}