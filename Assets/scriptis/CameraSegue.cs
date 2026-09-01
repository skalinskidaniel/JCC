using UnityEngine;

/// <summary>
/// Script respons�vel por fazer a c�mera seguir um alvo (como o jogador) de forma suave.
/// Deve ser anexado ao objeto Main Camera.
/// </summary>
public class CameraSegue : MonoBehaviour
{
    // [SerializeField] permite que a vari�vel apare�a no Inspector da Unity,
    // mas o 'private' impede que outros scripts alterem esse valor acidentalmente (Encapsulamento).

    [Header("Alvo Principal")]
    [Tooltip("Arraste o objeto que a c�mera deve seguir (ex: o Jogador) para c�.")]
    [SerializeField] private Transform alvo;

    [Header("Configura��es de Posi��o")]
    [Tooltip("A dist�ncia exata que a c�mera vai manter do alvo (Eixos X, Y e Z).")]
    [SerializeField] private Vector3 distanciaOffset = new Vector3(0, 5, -50);

    [Tooltip("Velocidade com que a c�mera acompanha o alvo. Valores maiores deixam a c�mera mais r�pida/r�gida.")]
    [SerializeField] private float velocidadeSuavizacao = 5f;

    [Header("Configura��es de Rota��o")]
    [Tooltip("Marque esta caixa se quiser que a c�mera sempre gire para 'encarar' o alvo.")]
    [SerializeField] private bool olharParaOAlvo = false;

    /// <summary>
    /// Usamos o LateUpdate em vez do Update cl�ssico para movimentar c�meras.
    /// O LateUpdate roda DEPOIS de todos os Updates e c�lculos de F�sica (FixedUpdate).
    /// Isso garante que a bola j� se moveu neste frame antes de movermos a c�mera, evitando "tremedeiras" na tela.
    /// </summary>
    private void LateUpdate()
    {
        // Trava de seguran�a: se o aluno esquecer de arrastar o alvo no Inspector, 
        // o script avisa no Console e para a execu��o, evitando que o jogo trave com erros vermelhos.
        if (alvo == null)
        {
            Debug.LogWarning("A c�mera n�o tem um alvo para seguir! V� no Inspector e arraste o jogador.", this);
            return;
        }

        // 1. Posi��o Alvo: Onde a c�mera DEVERIA estar neste exato momento? 
        // (� a posi��o atual da bola + a dist�ncia que configuramos)
        Vector3 posicaoDesejada = alvo.position + distanciaOffset;

        // 2. Transi��o Suave (O famoso "Lerp"):
        // O Vector3.Lerp calcula um ponto intermedi�rio entre onde a c�mera est� e para onde ela quer ir.
        // Multiplicar por Time.deltaTime � o segredo dos jogos profissionais: isso garante que a c�mera 
        // se mova na mesma velocidade em qualquer celular, seja ele muito r�pido ou muito lento.
        Vector3 posicaoSuavizada = Vector3.Lerp(transform.position, posicaoDesejada, velocidadeSuavizacao * Time.deltaTime);

        // 3. Finalmente, aplicamos a nova posi��o calculada � nossa c�mera
        transform.position = posicaoSuavizada;

        // 4. Efeito opcional: Fazer a c�mera virar como um "pesco�o" para olhar para a bola
        if (olharParaOAlvo)
        {
            transform.LookAt(alvo);
        }
    }
}
