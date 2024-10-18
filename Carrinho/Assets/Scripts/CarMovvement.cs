using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovvement : MonoBehaviour
{
    public float MaxSpeed = 10f;  // Velocidade m�xima
    public float acceleration = 5f;  // Acelera��o
    public float steering = 2f;  // Sensibilidade da dire��o

    private Rigidbody2D rigidbody2D;

    private float inputX;  // Entrada no eixo X (horizontal)
    private float inputY;  // Entrada no eixo Y (vertical)

    private void Start()
    {
        // Verifica��o inicial
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D n�o encontrado!");
        }
    }

    private void Update()
    {
        // Captura a entrada do jogador
        inputX = Input.GetAxis("Horizontal");  // Dire��o (esquerda/direita)
        inputY = Input.GetAxis("Vertical");    // Acelera��o (frente/tr�s)

        // Aplica��o da acelera��o (movimento para frente e para tr�s)
        Vector2 forwardForce = transform.right * (inputY * acceleration);  // Aplica no eixo direito (movimento para frente no eixo X)
        rigidbody2D.AddForce(forwardForce);

        Debug.Log("Acelerando com for�a: " + forwardForce);  // Verifica se a for�a est� sendo aplicada

        // Aplicando a rota��o baseada na entrada horizontal (esquerda/direita)
        float steeringAmount = inputX * steering;
        rigidbody2D.MoveRotation(rigidbody2D.rotation - steeringAmount);  // Aplica��o correta da rota��o

        // Limita a velocidade m�xima
        if (rigidbody2D.velocity.magnitude > MaxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxSpeed;
        }

        // Reduz o deslizamento lateral (drift)
        Vector2 forwardVelocity = transform.right * Vector2.Dot(rigidbody2D.velocity, transform.right);  // Alterado para o eixo X
        Vector2 rightVelocity = transform.up * Vector2.Dot(rigidbody2D.velocity, transform.up);

        // Aplica uma for�a contr�ria ao drift para reduzir o efeito de derrapagem
        rigidbody2D.velocity = forwardVelocity + rightVelocity * 0.2f;  // Reduzimos o impacto da for�a lateral

        // Visualiza a posi��o e for�a aplicada (debug)
        Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + forwardForce, Color.green);
    }
}