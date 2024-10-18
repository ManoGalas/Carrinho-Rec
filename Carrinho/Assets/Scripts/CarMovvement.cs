using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovvement : MonoBehaviour
{
    public float MaxSpeed = 10f;  // Velocidade máxima
    public float acceleration = 5f;  // Aceleração
    public float steering = 2f;  // Sensibilidade da direção

    private Rigidbody2D rigidbody2D;

    private float inputX;  // Entrada no eixo X (horizontal)
    private float inputY;  // Entrada no eixo Y (vertical)

    private void Start()
    {
        // Verificação inicial
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D não encontrado!");
        }
    }

    private void Update()
    {
        // Captura a entrada do jogador
        inputX = Input.GetAxis("Horizontal");  // Direção (esquerda/direita)
        inputY = Input.GetAxis("Vertical");    // Aceleração (frente/trás)

        // Aplicação da aceleração (movimento para frente e para trás)
        Vector2 forwardForce = transform.right * (inputY * acceleration);  // Aplica no eixo direito (movimento para frente no eixo X)
        rigidbody2D.AddForce(forwardForce);

        Debug.Log("Acelerando com força: " + forwardForce);  // Verifica se a força está sendo aplicada

        // Aplicando a rotação baseada na entrada horizontal (esquerda/direita)
        float steeringAmount = inputX * steering;
        rigidbody2D.MoveRotation(rigidbody2D.rotation - steeringAmount);  // Aplicação correta da rotação

        // Limita a velocidade máxima
        if (rigidbody2D.velocity.magnitude > MaxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxSpeed;
        }

        // Reduz o deslizamento lateral (drift)
        Vector2 forwardVelocity = transform.right * Vector2.Dot(rigidbody2D.velocity, transform.right);  // Alterado para o eixo X
        Vector2 rightVelocity = transform.up * Vector2.Dot(rigidbody2D.velocity, transform.up);

        // Aplica uma força contrária ao drift para reduzir o efeito de derrapagem
        rigidbody2D.velocity = forwardVelocity + rightVelocity * 0.2f;  // Reduzimos o impacto da força lateral

        // Visualiza a posição e força aplicada (debug)
        Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + forwardForce, Color.green);
    }
}