using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Interface IRaceParticipant: define a participa��o de um carro na corrida, com m�todos para atualizar posi��o, incrementar volta e obter a volta atual.
public interface IRaceParticipant
{
    void UpdatePosition(int position);  // Atualiza a posi��o do carro na corrida.
    void IncrementLap();                // Incrementa o n�mero de voltas completadas pelo carro.
    int GetCurrentLap();                // Retorna a volta atual do carro.
}

// Interface IDriving: define as a��es de dire��o de um carro, como acelerar, frear e direcionar.
public interface IDriving
{
    void Accelerate(float amount);    // M�todo para acelerar o carro.
    void Brake(float amount);         // M�todo para frear o carro.
    void Steer(float direction);      // M�todo para controlar a dire��o do carro.
}

// Classe CarMovvement: implementa as interfaces IRaceParticipant e IDriving para controlar o movimento e a participa��o do carro na corrida.
public class CarMovvement : MonoBehaviour, IRaceParticipant, IDriving
{
    // Propriedades p�blicas configur�veis no editor
    public float MaxSpeed = 10f;            // Velocidade m�xima do carro.
    public float acceleration = 5f;         // For�a de acelera��o do carro.
    public float steering = 2f;             // Intensidade da dire��o do carro.
    public int totalLaps = 3;               // N�mero total de voltas na corrida.

    // Componentes privados e vari�veis internas
    private Rigidbody2D rigidbody2D;        // Componente de f�sica para controlar o movimento.
    private float inputX;                   // Entrada horizontal (dire��o).
    private float inputY;                   // Entrada vertical (acelera��o).

    private int currentLap = 1;             // Volta atual do carro.
    private int positionInRace = 1;         // Posi��o atual do carro na corrida.

    // Elementos de UI para exibir informa��es ao jogador
    public Text speedText;                  // Texto que exibe a velocidade atual.
    public Slider speedSlider;              // Slider que indica a velocidade.
    public Text lapText;                    // Texto que exibe a volta atual.
    public Text positionText;               // Texto que exibe a posi��o na corrida.

    // M�todo Start: inicializa os componentes e configura o UI.
    private void Start()
    {
        // Obt�m o componente Rigidbody2D para aplicar for�as f�sicas
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.Log("Rigidbody2D n�o encontrado!");  // Mostra erro caso o componente n�o esteja presente.
        }

        // Verifica se os elementos de UI foram atribu�dos
        if (speedText == null || speedSlider == null || lapText == null || positionText == null)
        {
            Debug.Log("Elementos de UI n�o atribu�dos!");  // Mostra erro se algum UI estiver ausente.
        }
        else
        {
            speedSlider.maxValue = MaxSpeed;  // Define o valor m�ximo do slider de velocidade.
            UpdateUI();                       // Atualiza a interface com as informa��es iniciais.
        }
    }

    // M�todo Update: executa a cada frame, capturando entrada e atualizando o movimento.
    private void Update()
    {
        inputX = Input.GetAxis("Horizontal"); // Captura a entrada para a dire��o.
        inputY = Input.GetAxis("Vertical");   // Captura a entrada para acelera��o.

        // Usando os m�todos da interface IDriving para controlar o carro
        Accelerate(inputY);                   // Acelera de acordo com a entrada.
        Steer(inputX);                        // Altera a dire��o de acordo com a entrada.

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

    // Implementa��o do m�todo Accelerate da interface IDriving
    public void Accelerate(float amount)
    {
        Vector2 forwardForce = transform.right * (amount * acceleration);  // Calcula a for�a de acelera��o.
        rigidbody2D.AddForce(forwardForce);                                // Aplica a for�a no Rigidbody2D.

        // Limita a velocidade ao valor m�ximo
        if (rigidbody2D.velocity.magnitude > MaxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxSpeed;
        }

        // Linha de depura��o para visualizar a for�a de acelera��o no editor
        Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + forwardForce, Color.green);
    }

    // Implementa��o do m�todo Brake da interface IDriving
    public void Brake(float amount)
    {
        rigidbody2D.velocity = rigidbody2D.velocity * (1 - amount);  // Reduz a velocidade multiplicando pela quantidade de frenagem.
    }

    // Implementa��o do m�todo Steer da interface IDriving
    public void Steer(float direction)
    {
        float steeringAmount = direction * steering;                   // Calcula o valor de dire��o baseado na entrada.
        rigidbody2D.MoveRotation(rigidbody2D.rotation - steeringAmount);  // Altera a rota��o do carro.
    }

    // Implementa��o do m�todo UpdatePosition da interface IRaceParticipant
    public void UpdatePosition(int position)
    {
        positionInRace = position;  // Atualiza a posi��o na corrida.
        UpdateUI();                 // Atualiza o UI com a nova posi��o.
    }

    // Implementa��o do m�todo IncrementLap da interface IRaceParticipant
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

    // Implementa��o do m�todo GetCurrentLap da interface IRaceParticipant
    public int GetCurrentLap()
    {
        return currentLap;  // Retorna a volta atual do carro.
    }

    // M�todo UpdateUI: atualiza os elementos de UI com as informa��es mais recentes.
    private void UpdateUI()
    {
        if (lapText != null)
        {
            lapText.text = "Volta: " + currentLap + "/" + totalLaps;  // Atualiza a exibi��o da volta atual.
        }
        if (positionText != null)
        {
            positionText.text = "Posi��o: " + positionInRace;  // Atualiza a exibi��o da posi��o atual.
        }
    }
}