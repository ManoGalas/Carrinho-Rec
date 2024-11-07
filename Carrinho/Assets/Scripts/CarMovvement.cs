using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Interface IRaceParticipant: define a participação de um carro na corrida, com métodos para atualizar posição, incrementar volta e obter a volta atual.
public interface IRaceParticipant
{
    void UpdatePosition(int position);  // Atualiza a posição do carro na corrida.
    void IncrementLap();                // Incrementa o número de voltas completadas pelo carro.
    int GetCurrentLap();                // Retorna a volta atual do carro.
}

// Interface IDriving: define as ações de direção de um carro, como acelerar, frear e direcionar.
public interface IDriving
{
    void Accelerate(float amount);    // Método para acelerar o carro.
    void Brake(float amount);         // Método para frear o carro.
    void Steer(float direction);      // Método para controlar a direção do carro.
}

// Classe CarMovvement: implementa as interfaces IRaceParticipant e IDriving para controlar o movimento e a participação do carro na corrida.
public class CarMovvement : MonoBehaviour, IRaceParticipant, IDriving
{
    // Propriedades públicas configuráveis no editor
    public float MaxSpeed = 10f;            // Velocidade máxima do carro.
    public float acceleration = 5f;         // Força de aceleração do carro.
    public float steering = 2f;             // Intensidade da direção do carro.
    public int totalLaps = 3;               // Número total de voltas na corrida.

    // Componentes privados e variáveis internas
    private Rigidbody2D rigidbody2D;        // Componente de física para controlar o movimento.
    private float inputX;                   // Entrada horizontal (direção).
    private float inputY;                   // Entrada vertical (aceleração).

    private int currentLap = 1;             // Volta atual do carro.
    private int positionInRace = 1;         // Posição atual do carro na corrida.

    // Elementos de UI para exibir informações ao jogador
    public Text speedText;                  // Texto que exibe a velocidade atual.
    public Slider speedSlider;              // Slider que indica a velocidade.
    public Text lapText;                    // Texto que exibe a volta atual.
    public Text positionText;               // Texto que exibe a posição na corrida.

    // Método Start: inicializa os componentes e configura o UI.
    private void Start()
    {
        // Obtém o componente Rigidbody2D para aplicar forças físicas
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.Log("Rigidbody2D não encontrado!");  // Mostra erro caso o componente não esteja presente.
        }

        // Verifica se os elementos de UI foram atribuídos
        if (speedText == null || speedSlider == null || lapText == null || positionText == null)
        {
            Debug.Log("Elementos de UI não atribuídos!");  // Mostra erro se algum UI estiver ausente.
        }
        else
        {
            speedSlider.maxValue = MaxSpeed;  // Define o valor máximo do slider de velocidade.
            UpdateUI();                       // Atualiza a interface com as informações iniciais.
        }
    }

    // Método Update: executa a cada frame, capturando entrada e atualizando o movimento.
    private void Update()
    {
        inputX = Input.GetAxis("Horizontal"); // Captura a entrada para a direção.
        inputY = Input.GetAxis("Vertical");   // Captura a entrada para aceleração.

        // Usando os métodos da interface IDriving para controlar o carro
        Accelerate(inputY);                   // Acelera de acordo com a entrada.
        Steer(inputX);                        // Altera a direção de acordo com a entrada.

        // Atualiza a UI com a velocidade atual
        float currentSpeed = rigidbody2D.velocity.magnitude; // Calcula a velocidade atual do carro.
        if (speedText != null)
        {
            speedText.text = "Velocidade: " + Mathf.RoundToInt(currentSpeed) + " km/h";  // Exibe a velocidade arredondada.
        }
        if (speedSlider != null)
        {
            speedSlider.value = currentSpeed;  // Atualiza o slider com a velocidade.
        }

        // Reduz o deslizamento lateral (drift), controlando a estabilidade do carro.
        Vector2 forwardVelocity = transform.right * Vector2.Dot(rigidbody2D.velocity, transform.right);
        Vector2 rightVelocity = transform.up * Vector2.Dot(rigidbody2D.velocity, transform.up);

        rigidbody2D.velocity = forwardVelocity + rightVelocity * 0.2f;  // Controla o deslizamento lateral reduzindo sua magnitude.
    }

    // Implementação do método Accelerate da interface IDriving
    public void Accelerate(float amount)
    {
        Vector2 forwardForce = transform.right * (amount * acceleration);  // Calcula a força de aceleração.
        rigidbody2D.AddForce(forwardForce);                                // Aplica a força no Rigidbody2D.

        // Limita a velocidade ao valor máximo
        if (rigidbody2D.velocity.magnitude > MaxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxSpeed;
        }

        // Linha de depuração para visualizar a força de aceleração no editor
        Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + forwardForce, Color.green);
    }

    // Implementação do método Brake da interface IDriving
    public void Brake(float amount)
    {
        rigidbody2D.velocity = rigidbody2D.velocity * (1 - amount);  // Reduz a velocidade multiplicando pela quantidade de frenagem.
    }

    // Implementação do método Steer da interface IDriving
    public void Steer(float direction)
    {
        float steeringAmount = direction * steering;                   // Calcula o valor de direção baseado na entrada.
        rigidbody2D.MoveRotation(rigidbody2D.rotation - steeringAmount);  // Altera a rotação do carro.
    }

    // Implementação do método UpdatePosition da interface IRaceParticipant
    public void UpdatePosition(int position)
    {
        positionInRace = position;  // Atualiza a posição na corrida.
        UpdateUI();                 // Atualiza o UI com a nova posição.
    }

    // Implementação do método IncrementLap da interface IRaceParticipant
    public void IncrementLap()
    {
        if (currentLap < totalLaps)
        {
            currentLap++;  // Incrementa a volta atual.
            UpdateUI();    // Atualiza a UI para refletir a nova volta.
        }
        else
        {
            Debug.Log("Corrida Completa!");  // Exibe mensagem se todas as voltas foram completadas.
        }
    }

    // Implementação do método GetCurrentLap da interface IRaceParticipant
    public int GetCurrentLap()
    {
        return currentLap;  // Retorna a volta atual do carro.
    }

    // Método UpdateUI: atualiza os elementos de UI com as informações mais recentes.
    private void UpdateUI()
    {
        if (lapText != null)
        {
            lapText.text = "Volta: " + currentLap + "/" + totalLaps;  // Atualiza a exibição da volta atual.
        }
        if (positionText != null)
        {
            positionText.text = "Posição: " + positionInRace;  // Atualiza a exibição da posição atual.
        }
    }
}