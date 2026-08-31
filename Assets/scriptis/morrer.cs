using UnityEngine;
using UnityEngine.SceneManagement;

public class morrer : MonoBehaviour
{
    [Tooltip("Tempo de espera (em segundos) antes de reiniciar a fase. Deixe 0 para reiniciar na hora.")]
    public float atrasoParaReiniciar = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("morre"))
        {
            ReiniciarFase();
        }
    }

    void ReiniciarFase()
    {
        Scene cenaAtual = SceneManager.GetActiveScene();

        if (atrasoParaReiniciar > 0f)
        {
            Invoke(nameof(CarregarCenaAtual), atrasoParaReiniciar);
        }
        else
        {
            SceneManager.LoadScene(cenaAtual.buildIndex);
        }
    }

    void CarregarCenaAtual()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}