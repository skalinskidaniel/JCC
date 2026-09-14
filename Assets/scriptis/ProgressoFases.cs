using UnityEngine;


public static class ProgressoFases
{
    private const string CHAVE_FASE_MAXIMA = "FaseMaximaDesbloqueada";
    public static int FaseMaximaDesbloqueada
    {
        get => PlayerPrefs.GetInt(CHAVE_FASE_MAXIMA, 1);
        private set => PlayerPrefs.SetInt(CHAVE_FASE_MAXIMA, value);
    }
    
    public static bool FaseEstaDesbloqueada(int numeroFase)
    {
        return numeroFase <= FaseMaximaDesbloqueada;
    }
    
    public static void DesbloquearFase(int numeroFase)
    {
        if (numeroFase > FaseMaximaDesbloqueada)
        {
            FaseMaximaDesbloqueada = numeroFase;
            PlayerPrefs.Save();
        }
    }

    public static void ConcluirFase(int numeroFaseConcluida)
    {
        DesbloquearFase(numeroFaseConcluida + 1);
    }
    public static void ResetarProgresso()
    {
        PlayerPrefs.DeleteKey(CHAVE_FASE_MAXIMA);
        PlayerPrefs.Save();
    }
}