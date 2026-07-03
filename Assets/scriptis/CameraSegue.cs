using UnityEngine;

/// <summary>
/// Script responsável por fazer a câmera seguir um alvo (como o jogador) de forma suave.
/// Deve ser anexado ao objeto Main Camera.
/// </summary>
public class CameraSegue : MonoBehaviour
{
    // [SerializeField] permite que a variável apareça no Inspector da Unity,
    // mas o 'private' impede que outros scripts alterem esse valor acidentalmente (Encapsulamento).

    [Header("Alvo Principal")]
    [Tooltip("Arraste o objeto que a câmera deve seguir (ex: o Jogador) para cá.")]
    [SerializeField] private Transform alvo;

    [Header("Configurações de Posição")]
    [Tooltip("A distância exata que a câmera vai manter do alvo (Eixos X, Y e Z).")]
    [SerializeField] private Vector3 distanciaOffset = new Vector3(0, 5, -10);

    [Tooltip("Velocidade com que a câmera acompanha o alvo. Valores maiores deixam a câmera mais rápida/rígida.")]
    [SerializeField] private float velocidadeSuavizacao = 5f;

    [Header("Configurações de Rotação")]
    [Tooltip("Marque esta caixa se quiser que a câmera sempre gire para 'encarar' o alvo.")]
    [SerializeField] private bool olharParaOAlvo = false;

    /// <summary>
    /// Usamos o LateUpdate em vez do Update clássico para movimentar câmeras.
    /// O LateUpdate roda DEPOIS de todos os Updates e cálculos de Física (FixedUpdate).
    /// Isso garante que a bola já se moveu neste frame antes de movermos a câmera, evitando "tremedeiras" na tela.
    /// </summary>
    private void LateUpdate()
    {
        // Trava de segurança: se o aluno esquecer de arrastar o alvo no Inspector, 
        // o script avisa no Console e para a execução, evitando que o jogo trave com erros vermelhos.
        if (alvo == null)
        {
            Debug.LogWarning("A câmera não tem um alvo para seguir! Vá no Inspector e arraste o jogador.", this);
            return;
        }

        // 1. Posição Alvo: Onde a câmera DEVERIA estar neste exato momento? 
        // (É a posição atual da bola + a distância que configuramos)
        Vector3 posicaoDesejada = alvo.position + distanciaOffset;

        // 2. Transição Suave (O famoso "Lerp"):
        // O Vector3.Lerp calcula um ponto intermediário entre onde a câmera está e para onde ela quer ir.
        // Multiplicar por Time.deltaTime é o segredo dos jogos profissionais: isso garante que a câmera 
        // se mova na mesma velocidade em qualquer celular, seja ele muito rápido ou muito lento.
        Vector3 posicaoSuavizada = Vector3.Lerp(transform.position, posicaoDesejada, velocidadeSuavizacao * Time.deltaTime);

        // 3. Finalmente, aplicamos a nova posição calculada à nossa câmera
        transform.position = posicaoSuavizada;

        // 4. Efeito opcional: Fazer a câmera virar como um "pescoço" para olhar para a bola
        if (olharParaOAlvo)
        {
            transform.LookAt(alvo);
        }
    }
}
