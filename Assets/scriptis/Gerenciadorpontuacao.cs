using System;
using UnityEngine;

/// <summary>
/// Controla a pontuacao da fase atual. Coloque este script em UM objeto vazio
/// por fase (ex: "GerenciadorPontuacao"). Como e um objeto normal da cena
/// (nao usa DontDestroyOnLoad), a pontuacao zera sozinha quando a fase reinicia.
/// </summary>
public class GerenciadorPontuacao : MonoBehaviour
{
  
    public static GerenciadorPontuacao Instance { get; private set; }

    public static event Action<int> AoPontuarMudar;

    
    private int pontosIniciais = 0;

    private int pontosAtuais;

    public int PontosAtuais => pontosAtuais;

    void Awake()
    {
       
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        pontosAtuais = pontosIniciais;
    }

    void Start()
    {
       
        AoPontuarMudar?.Invoke(pontosAtuais);
    }

   
    public void AdicionarPontos(int quantidade)
    {
        pontosAtuais += quantidade;
        AoPontuarMudar?.Invoke(pontosAtuais);
    }
}