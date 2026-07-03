using UnityEngine;
// 1. IMPORTANTE: Precisamos "avisar" o script que vamos usar as ferramentas do Novo Input System
using UnityEngine.InputSystem;

/// <summary>
/// Script responsável por movimentar um objeto (bola) usando física (torque) 
/// e interagir gerando novos objetos. 
/// Utiliza a classe C# gerada automaticamente pelo Input Action Asset.
/// </summary>
[RequireComponent(typeof(Rigidbody))] // Segurança didática: Garante que a Unity adicione um Rigidbody automaticamente para não dar erro se o aluno esquecer
public class MovimentoBola : MonoBehaviour
{
    [Header("Configurações de Física")]
    [Tooltip("Controla a força de rotação da bola. Valores maiores deixam a bola mais rápida.")]
    [SerializeField] private float velocidade = 10f;

    [Header("Configurações de Interação")]
    [Tooltip("Arraste para cá o Prefab que vai 'nascer' quando o jogador apertar o botão de ação.")]
    [SerializeField] private GameObject objetoPrefab;

    // Variável interna para guardar a referência do nosso componente de física
    private Rigidbody rb;

    // 2. A NOSSA CLASSE GERADA: 
    // ATENÇÃO ALUNOS: "ControlesDoJogo" deve ser o nome exato do arquivo .inputactions de vocês!
    // Essa variável vai guardar todo o nosso mapa de botões e sensores.
    private Controles input;

    /// <summary>
    /// O Awake é chamado assim que o jogo começa, antes mesmo do primeiro frame.
    /// É o melhor lugar para "preparar o terreno" e configurar nossas variáveis.
    /// </summary>
    private void Awake()
    {
        // Pega o componente Rigidbody que está "grudado" neste mesmo objeto
        rb = GetComponent<Rigidbody>();

        // "Constrói"/Instacia a nossa classe de controles para que possamos usá-la no script
        input = new Controles();

        // Verifica se o dispositivo atual possui um acelerômetro físico
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            // Força a Unity a ligar o sensor e começar a ler os dados
            UnityEngine.InputSystem.InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
        }
    }

    /// <summary>
    /// OnEnable é ativado toda vez que este objeto "nasce" ou é ligado na cena.
    /// No Novo Input System, OS CONTROLES COMEÇAM DESLIGADOS por padrão para economizar memória.
    /// Precisamos ligá-los explicitamente aqui.
    /// </summary>
    private void OnEnable()
    {
        // Liga o mapa de controles chamado "Gameplay" inteiro de uma vez
        input.Gameplay.Enable();
    }

    /// <summary>
    /// OnDisable é ativado quando o objeto é desligado ou destruído.
    /// É uma boa prática de programação desligar os controles quando não estamos usando.
    /// </summary>
    private void OnDisable()
    {
        input.Gameplay.Disable();
    }

    /// <summary>
    /// FixedUpdate é o coração da Física na Unity. Ele roda em um tempo fixo,
    /// garantindo que a bola não se mova de forma bizarra se o computador do jogador for lento.
    /// Usamos ele SEMPRE que fomos aplicar forças no Rigidbody.
    /// </summary>
    private void FixedUpdate()
    {
        // 3. LENDO O INPUT DE MOVIMENTO:
        // Lemos o valor que está vindo do teclado (WASD) ou do acelerômetro do celular.
        // Como configuramos para Vector3, ele nos dá (X, Y, Z).
        Vector3 inputMovimento = input.Gameplay.Movimento.ReadValue<Vector3>();

        // Enviamos esse valor lido para a nossa função que faz a bola girar
        AplicarTorque(inputMovimento);
    }

    /// <summary>
    /// Função criada por nós para organizar o código. 
    /// Ela transforma a entrada do jogador em força física (Torque).
    /// </summary>
    private void AplicarTorque(Vector3 inputAtual)
    {
        // 4. TRADUZINDO OS EIXOS:
        // No teclado, W e S geram valores no eixo Y do Vector3. Mas no mundo 3D do jogo,
        // para ir para frente e para trás, precisamos girar no eixo Z.
        // Por isso, trocamos os eixos de lugar aqui.
        Vector3 direcaoTorque = new Vector3(
            inputAtual.y * velocidade, // O Y do controle vira a rotação (Z) no jogo
            0,                         // Não queremos que a bola pule (eixo Y do jogo)
            inputAtual.x * -velocidade // O X do controle continua sendo o X, mas invertido para girar certo
        );

        // Se o jogador estiver apertando alguma tecla (ou seja, se a direção não for zero)
        if (direcaoTorque != Vector3.zero)
        {
            // Aplica a força de giro (Torque) na bola
            rb.AddTorque(direcaoTorque, ForceMode.Force);
        }
    }

    /// <summary>
    /// O Update roda a cada frame (imagem) gerada na tela.
    /// É o lugar perfeito para checar se botões de ação rápida foram apertados.
    /// </summary>
    private void Update()
    {
        // 5. LENDO O BOTÃO DE AÇÃO:
        // "WasPressedThisFrame" verifica se o botão foi apertado EXATAMENTE neste milissegundo.
        // Isso evita que o jogo crie mil objetos se o jogador segurar o botão pressionado.
        if (input.Gameplay.Acao.WasPressedThisFrame())
        {
            CriarEProgramarDestruicaoObjeto();
        }
    }

    /// <summary>
    /// Instancia (cria) um novo objeto no mundo e agenda a destruição dele.
    /// </summary>
    private void CriarEProgramarDestruicaoObjeto()
    {
        // Trava de segurança: avisa no console se o aluno esqueceu de arrastar o Prefab no Inspector
        if (objetoPrefab == null)
        {
            Debug.LogWarning("Ops! Você esqueceu de arrastar o Prefab para o script no Inspector!", this);
            return; // Sai da função para não dar erro fatal no jogo
        }

        // Cria uma cópia do objeto exatamente na mesma posição da bola
        GameObject clone = Instantiate(objetoPrefab, transform.position, Quaternion.identity);

        // Faz o novo objeto "grudar" na bola (vira filho dela)
        clone.transform.SetParent(transform, true);

        // Destrói este clone automaticamente após 1 segundo, para não lotar a memória do computador
        Destroy(clone, 1f);
    }

    /// <summary>
    /// O método OnGUI desenha elementos de interface simples diretamente na tela.
    /// Excelente para debugar variáveis no celular de forma rápida!
    /// </summary>
    private void OnGUI()
    {
        // 1. Aumenta o tamanho da fonte para que dê para ler na tela do celular
        GUI.skin.label.fontSize = 60;

        // 2. Lê o valor atual que está vindo do Input
        Vector3 debugMovimento = input.Gameplay.Movimento.ReadValue<Vector3>();

        // 3. Desenha o texto na tela. 
        // Rect(posição X, posição Y, largura, altura)
        GUI.Label(new Rect(50, 50, 1000, 200), "Input X: " + debugMovimento.x.ToString("F2"));
        GUI.Label(new Rect(50, 120, 1000, 200), "Input Y: " + debugMovimento.y.ToString("F2"));
        GUI.Label(new Rect(50, 190, 1000, 200), "Input Z: " + debugMovimento.z.ToString("F2"));
    }

}
