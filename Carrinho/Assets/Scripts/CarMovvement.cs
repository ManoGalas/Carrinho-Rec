using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovvement : MonoBehaviour
{
    public float MaxSpeed;
    public float acceleration;
    public float steering;

    Rigidbody2D rigidbody2D;

    float X;
    float Y = 1;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        X = Input.GetAxis("Horizontal");


        Vector2 speed = transform.up * (Y * acceleration);
        rigidbody2D.AddForce(speed);

        float direction = Vector2.Dot(rigidbody2D.velocity, rigidbody2D.GetRelativeVector(Vector2.up));

        if (acceleration > 0)
        {
            if (direction > 0)
            {
                rigidbody2D.rotation -= X * steering * (rigidbody2D.velocity.magnitude / MaxSpeed);
            }
            else
            {
                rigidbody2D.rotation += X * steering * (rigidbody2D.velocity.magnitude / MaxSpeed);
            }
        }

        float driftForce = Vector2.Dot(rigidbody2D.velocity, rigidbody2D.GetRelativeVector(Vector2.left)) * 2.0f;

        Vector2 relativeForce = Vector2.right * driftForce;

        rigidbody2D.AddForce(rigidbody2D.GetRelativeVector(relativeForce));

        if (rigidbody2D.velocity.magnitude > MaxSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * MaxSpeed;
        }

        Debug.DrawLine(rigidbody2D.position, rigidbody2D.GetRelativePoint(relativeForce),Color.green);
    }

   
}
