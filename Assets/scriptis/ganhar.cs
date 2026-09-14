using UnityEngine;
using UnityEngine.SceneManagement;

public class ganhar : MonoBehaviour
{
   
    public string proximafase = "fase2";
    public int numeroFaseAtual = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ganhar"))
        {

            ProgressoFases.ConcluirFase(numeroFaseAtual);
            SceneManager.LoadScene(proximafase);
        }

        
    }
}