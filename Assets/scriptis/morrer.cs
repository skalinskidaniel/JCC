using UnityEngine;
using UnityEngine.SceneManagement;

public class morrer : MonoBehaviour
{
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