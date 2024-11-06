using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;  // Adicione o Photon

// Interface IDrivable
public interface IDrivable
{
    void Accelerate(float amount);
    void Brake();
    void Steer(float direction);
}

// Classe CarMovement que implementa IDrivable
public class CarMovvement : MonoBehaviourPun, IDrivable
{
    public float MaxSpeed = 10f;  // Velocidade máxima
    public float acceleration = 5f;  // Aceleração
    public float steering = 2f;  // Sensibilidade da direção

    Rigidbody2D rigidbody2D;

    private float inputX;  // Entrada no eixo X (horizontal)
    private float inputY;  // Entrada no eixo Y (vertical)

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D não encontrado!");
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            inputX = Input.GetAxis("Horizontal");
            inputY = Input.GetAxis("Vertical");

            photonView.RPC("MoveCar", RpcTarget.All, inputX, inputY);
        }
    }

    [PunRPC]
    public void MoveCar(float inputX, float inputY)
    {
        Accelerate(inputY);
        Steer(inputX);

        if (rigidbody2D.velocity.magnitude > MaxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxSpeed;
        }

        Vector2 forwardVelocity = transform.right * Vector2.Dot(rigidbody2D.velocity, transform.right);
        Vector2 rightVelocity = transform.up * Vector2.Dot(rigidbody2D.velocity, transform.up);
        rigidbody2D.velocity = forwardVelocity + rightVelocity * 0.2f;

        Debug.DrawLine(rigidbody2D.position, rigidbody2D.position + forwardVelocity, Color.green);
    }

    public void Accelerate(float amount)
    {
        Vector2 forwardForce = transform.right * (amount * acceleration);
        rigidbody2D.AddForce(forwardForce);
        Debug.Log("Acelerando com força: " + forwardForce);
    }

    public void Brake()
    {
        rigidbody2D.velocity = Vector2.zero;
    }

    public void Steer(float direction)
    {
        float steeringAmount;
    }
}