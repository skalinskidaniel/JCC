using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFases : MonoBehaviour
{
    
    public void CarregarFase(string nomeDaCena)
    {
    

        SceneManager.LoadScene(nomeDaCena);
    }

    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

 
    public void ReiniciarFaseAtual()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

  
    public void VoltarParaMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}