using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;  // Biblioteca matemática adicional para Unity.
using UnityEngine;

// Classe CameraControl: controla a rotação da câmera com base em colisões, permitindo que a câmera gire para um ângulo predefinido.
public class CameraControl : MonoBehaviour
{
    // Um array de ângulos (em Z) para os quais a câmera irá rotacionar com base no número de colisões.
    public float[] RotZ;

    // Contador para monitorar o número de colisões que ocorreram.
    int triggerCount = 0;

    // Referência à câmera principal do jogo.
    Camera Cam;

    // Flag para permitir ou não a rotação da câmera.
    bool CanRotate;

    // A velocidade de rotação da câmera.
    public float rotSpeed;

    // Método Start: é chamado uma vez quando o script começa a execução.
    // Aqui, obtemos a referência para a câmera principal.
    private void Start()
    {
        Cam = Camera.main;  // Obtém a câmera principal da cena.
    }

    // Método Update: é chamado a cada frame.
    // Aqui, a rotação da câmera é ajustada, caso a flag CanRotate esteja ativa.
    private void Update()
    {
        if (CanRotate)  // Verifica se a rotação está permitida.
        {
            // Armazena a rotação atual da câmera.
            Quaternion camRot = Cam.transform.rotation;

            // Cria a rotação desejada com base no valor de RotZ do array, utilizando o número de colisões (triggerCount).
            Quaternion desRot = Quaternion.Euler(0, 0, RotZ[triggerCount - 1]);

            // Interpola suavemente a rotação atual para a rotação desejada, com base na velocidade rotSpeed.
            Cam.transform.rotation = Quaternion.Lerp(camRot, desRot, Time.deltaTime * rotSpeed);

            // Se a rotação da câmera estiver próxima da desejada, desativa a rotação.
            if (camRot.z == desRot.z)
            {
                CanRotate = false;  // Impede a rotação ao alcançar a rotação desejada.
            }
        }
    }

    // Método OnTriggerEnter2D: é chamado quando o objeto com este script entra em colisão com outro objeto 2D que tenha um Collider2D.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");  // Exibe uma mensagem no console quando ocorre a colisão.

        // Se o número de colisões já foi 4, reinicia o contador.
        if (triggerCount == 4)
            triggerCount = 0;

        // Incrementa o contador de colisões.
        triggerCount += 1;

        // Permite a rotação da câmera após a colisão.
        CanRotate = true;
    }
}
