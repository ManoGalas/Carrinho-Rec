using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;  // Biblioteca matem�tica adicional para Unity.
using UnityEngine;

// Classe CameraControl: controla a rota��o da c�mera com base em colis�es, permitindo que a c�mera gire para um �ngulo predefinido.
public class CameraControl : MonoBehaviour
{
    // Um array de �ngulos (em Z) para os quais a c�mera ir� rotacionar com base no n�mero de colis�es.
    public float[] RotZ;

    // Contador para monitorar o n�mero de colis�es que ocorreram.
    int triggerCount = 0;

    // Refer�ncia � c�mera principal do jogo.
    Camera Cam;

    // Flag para permitir ou n�o a rota��o da c�mera.
    bool CanRotate;

    // A velocidade de rota��o da c�mera.
    public float rotSpeed;

    // M�todo Start: � chamado uma vez quando o script come�a a execu��o.
    // Aqui, obtemos a refer�ncia para a c�mera principal.
    private void Start()
    {
        Cam = Camera.main;  // Obt�m a c�mera principal da cena.
    }

    // M�todo Update: � chamado a cada frame.
    // Aqui, a rota��o da c�mera � ajustada, caso a flag CanRotate esteja ativa.
    private void Update()
    {
        if (CanRotate)  // Verifica se a rota��o est� permitida.
        {
            // Armazena a rota��o atual da c�mera.
            Quaternion camRot = Cam.transform.rotation;

            // Cria a rota��o desejada com base no valor de RotZ do array, utilizando o n�mero de colis�es (triggerCount).
            Quaternion desRot = Quaternion.Euler(0, 0, RotZ[triggerCount - 1]);

            // Interpola suavemente a rota��o atual para a rota��o desejada, com base na velocidade rotSpeed.
            Cam.transform.rotation = Quaternion.Lerp(camRot, desRot, Time.deltaTime * rotSpeed);

            // Se a rota��o da c�mera estiver pr�xima da desejada, desativa a rota��o.
            if (camRot.z == desRot.z)
            {
                CanRotate = false;  // Impede a rota��o ao alcan�ar a rota��o desejada.
            }
        }
    }

    // M�todo OnTriggerEnter2D: � chamado quando o objeto com este script entra em colis�o com outro objeto 2D que tenha um Collider2D.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");  // Exibe uma mensagem no console quando ocorre a colis�o.

        // Se o n�mero de colis�es j� foi 4, reinicia o contador.
        if (triggerCount == 4)
            triggerCount = 0;

        // Incrementa o contador de colis�es.
        triggerCount += 1;

        // Permite a rota��o da c�mera ap�s a colis�o.
        CanRotate = true;
    }
}
