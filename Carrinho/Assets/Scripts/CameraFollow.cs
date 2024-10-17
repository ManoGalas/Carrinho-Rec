using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 Offset;

    public float smoothness;

    void Update()
    {
        Vector3 delayedPos = Vector3.Lerp(transform.position,target.position, smoothness);

        transform.position = delayedPos + Offset;
    }
}
