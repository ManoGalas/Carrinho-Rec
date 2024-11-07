using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe CameraFollow: controla o movimento da câmera, fazendo-a seguir um alvo (target) com um determinado atraso suave e um offset.
public class CameraFollow : MonoBehaviour
{
    // A referência para o objeto que a câmera deve seguir.
    public Transform target;

    // A posição offset que será adicionada à posição da câmera, ajustando sua distância ou altura em relação ao alvo.
    public Vector3 Offset;

    // A suavidade com que a câmera segue o alvo (quanto maior o valor, mais suave será o movimento).
    public float smoothness;

    // Método Update: é chamado a cada frame e controla a atualização da posição da câmera.
    void Update()
    {
        // Calcula a posição desejada para a câmera com suavização, movendo a câmera gradualmente em direção ao alvo.
        Vector3 delayedPos = Vector3.Lerp(transform.position, target.position, smoothness);

        // Atualiza a posição da câmera, adicionando o offset para ajustar sua posição em relação ao alvo.
        transform.position = delayedPos + Offset;
    }
}
