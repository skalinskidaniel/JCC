using UnityEngine;
// 1. IMPORTANTE: Precisamos "avisar" o script que vamos usar as ferramentas do Novo Input System
using UnityEngine.InputSystem;

/// <summary>
/// Script respons�vel por movimentar um objeto (bola) usando f�sica (torque) 
/// e interagir gerando novos objetos. 
/// Utiliza a classe C# gerada automaticamente pelo Input Action Asset.
/// </summary>
[RequireComponent(typeof(Rigidbody))] // Seguran�a did�tica: Garante que a Unity adicione um Rigidbody automaticamente para n�o dar erro se o aluno esquecer
public class MovimentoBola : MonoBehaviour
{
    [Header("Configura��es de F�sica")]
    [Tooltip("Controla a for�a de rota��o da bola. Valores maiores deixam a bola mais r�pida.")]
    [SerializeField] private float velocidade = 10f;

    [Header("Configura��es de Intera��o")]
    [Tooltip("Arraste para c� o Prefab que vai 'nascer' quando o jogador apertar o bot�o de a��o.")]
    [SerializeField] private GameObject objetoPrefab;

    // Vari�vel interna para guardar a refer�ncia do nosso componente de f�sica
    private Rigidbody rb;

    // 2. A NOSSA CLASSE GERADA: 
    // ATEN��O ALUNOS: "ControlesDoJogo" deve ser o nome exato do arquivo .inputactions de voc�s!
    // Essa vari�vel vai guardar todo o nosso mapa de bot�es e sensores.
    private Controles input;

    /// <summary>
    /// O Awake � chamado assim que o jogo come�a, antes mesmo do primeiro frame.
    /// � o melhor lugar para "preparar o terreno" e configurar nossas vari�veis.
    /// </summary>
    private void Awake()
    {
        // Pega o componente Rigidbody que est� "grudado" neste mesmo objeto
        rb = GetComponent<Rigidbody>();

        // "Constr�i"/Instacia a nossa classe de controles para que possamos us�-la no script
        input = new Controles();

        // Verifica se o dispositivo atual possui um aceler�metro f�sico
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            // For�a a Unity a ligar o sensor e come�ar a ler os dados
            UnityEngine.InputSystem.InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
        }
    }

    /// <summary>
    /// OnEnable � ativado toda vez que este objeto "nasce" ou � ligado na cena.
    /// No Novo Input System, OS CONTROLES COME�AM DESLIGADOS por padr�o para economizar mem�ria.
    /// Precisamos lig�-los explicitamente aqui.
    /// </summary>
    private void OnEnable()
    {
        // Liga o mapa de controles chamado "Gameplay" inteiro de uma vez
        input.Gameplay.Enable();
    }

    /// <summary>
    /// OnDisable � ativado quando o objeto � desligado ou destru�do.
    /// � uma boa pr�tica de programa��o desligar os controles quando n�o estamos usando.
    /// </summary>
    private void OnDisable()
    {
        input.Gameplay.Disable();
    }

    /// <summary>
    /// FixedUpdate � o cora��o da F�sica na Unity. Ele roda em um tempo fixo,
    /// garantindo que a bola n�o se mova de forma bizarra se o computador do jogador for lento.
    /// Usamos ele SEMPRE que fomos aplicar for�as no Rigidbody.
    /// </summary>
    private void FixedUpdate()
    {
        // 3. LENDO O INPUT DE MOVIMENTO:
        // Lemos o valor que est� vindo do teclado (WASD) ou do aceler�metro do celular.
        // Como configuramos para Vector3, ele nos d� (X, Y, Z).
        Vector3 inputMovimento = input.Gameplay.Movimento.ReadValue<Vector3>();

        // Enviamos esse valor lido para a nossa fun��o que faz a bola girar
        AplicarTorque(inputMovimento);
    }

    /// <summary>
    /// Fun��o criada por n�s para organizar o c�digo. 
    /// Ela transforma a entrada do jogador em for�a f�sica (Torque).
    /// </summary>
    private void AplicarTorque(Vector3 inputAtual)
    {
        // 4. TRADUZINDO OS EIXOS:
        // No teclado, W e S geram valores no eixo Y do Vector3. Mas no mundo 3D do jogo,
        // para ir para frente e para tr�s, precisamos girar no eixo Z.
        // Por isso, trocamos os eixos de lugar aqui.
        Vector3 direcaoTorque = new Vector3(
            inputAtual.y * velocidade, // O Y do controle vira a rota��o (Z) no jogo
            0,                         // N�o queremos que a bola pule (eixo Y do jogo)
            inputAtual.x * -velocidade // O X do controle continua sendo o X, mas invertido para girar certo
        );

        // Se o jogador estiver apertando alguma tecla (ou seja, se a dire��o n�o for zero)
        if (direcaoTorque != Vector3.zero)
        {
            // Aplica a for�a de giro (Torque) na bola
            rb.AddTorque(direcaoTorque, ForceMode.Force);
        }
    }

    /// <summary>
    /// O Update roda a cada frame (imagem) gerada na tela.
    /// � o lugar perfeito para checar se bot�es de a��o r�pida foram apertados.
    /// </summary>
    private void Update()
    {
        // 5. LENDO O BOT�O DE A��O:
        // "WasPressedThisFrame" verifica se o bot�o foi apertado EXATAMENTE neste milissegundo.
        // Isso evita que o jogo crie mil objetos se o jogador segurar o bot�o pressionado.
        if (input.Gameplay.Acao.WasPressedThisFrame())
        {
            CriarEProgramarDestruicaoObjeto();
        }
    }

    /// <summary>
    /// Instancia (cria) um novo objeto no mundo e agenda a destrui��o dele.
    /// </summary>
    private void CriarEProgramarDestruicaoObjeto()
    {
        // Trava de seguran�a: avisa no console se o aluno esqueceu de arrastar o Prefab no Inspector
        if (objetoPrefab == null)
        {
            Debug.LogWarning("Ops! Voc� esqueceu de arrastar o Prefab para o script no Inspector!", this);
            return; // Sai da fun��o para n�o dar erro fatal no jogo
        }

        // Cria uma c�pia do objeto exatamente na mesma posi��o da bola
        GameObject clone = Instantiate(objetoPrefab, transform.position, Quaternion.identity);

        // Faz o novo objeto "grudar" na bola (vira filho dela)
        clone.transform.SetParent(transform, true);

        // Destr�i este clone automaticamente ap�s 1 segundo, para n�o lotar a mem�ria do computador
        Destroy(clone, 1f);
    }

    /// <summary>
    /// O m�todo OnGUI desenha elementos de interface simples diretamente na tela.
    /// Excelente para debugar vari�veis no celular de forma r�pida!
    /// </summary>
    private void OnGUI()
    {
        // 1. Aumenta o tamanho da fonte para que d� para ler na tela do celular
        GUI.skin.label.fontSize = 60;

        // 2. L� o valor atual que est� vindo do Input
        Vector3 debugMovimento = input.Gameplay.Movimento.ReadValue<Vector3>();

        // 3. Desenha o texto na tela. 
        // Rect(posi��o X, posi��o Y, largura, altura)
       
    }

}
