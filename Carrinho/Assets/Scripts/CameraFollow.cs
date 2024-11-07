using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe CameraFollow: controla o movimento da c�mera, fazendo-a seguir um alvo (target) com um determinado atraso suave e um offset.
public class CameraFollow : MonoBehaviour
{
    // A refer�ncia para o objeto que a c�mera deve seguir.
    public Transform target;

    // A posi��o offset que ser� adicionada � posi��o da c�mera, ajustando sua dist�ncia ou altura em rela��o ao alvo.
    public Vector3 Offset;

    // A suavidade com que a c�mera segue o alvo (quanto maior o valor, mais suave ser� o movimento).
    public float smoothness;

    // M�todo Update: � chamado a cada frame e controla a atualiza��o da posi��o da c�mera.
    void Update()
    {
        // Calcula a posi��o desejada para a c�mera com suaviza��o, movendo a c�mera gradualmente em dire��o ao alvo.
        Vector3 delayedPos = Vector3.Lerp(transform.position, target.position, smoothness);

        // Atualiza a posi��o da c�mera, adicionando o offset para ajustar sua posi��o em rela��o ao alvo.
        transform.position = delayedPos + Offset;
    }
}
