using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Tyre : MonoBehaviour
{
    public float force = 3000;
    public float friction = 2;
    public float rotationSpeed = 90;
    [Range(0,1)]
    public float forceFlyingCoef = 0.25f;
    [Range(0, 1)]
    public float rotationFlyingCoef = 0.5f;
    public float radius = 0.5f;
    public float centrifugalForce = 2;

    protected Vector2 controller;
    protected Rigidbody rb;

    protected bool onGround;



    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected virtual void Update()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, radius + 0.05f);

        controller = Vector2.zero;
        UpdateController();

        // add force
        if (controller.x != 0) rb.AddForce(Forward() * Time.deltaTime * controller.x * force * (onGround ? 1 : forceFlyingCoef));

        // friction
        rb.velocity *= 1 - Time.deltaTime * friction * (onGround ? 1 : forceFlyingCoef);

        // rotate
        if (controller.y != 0)
        {
            Vector3 velocity = rb.velocity;
            velocity = transform.InverseTransformVector(velocity);
            transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * controller.y * rotationSpeed * (onGround ? 1 : rotationFlyingCoef));
            velocity = transform.TransformVector(velocity);
            rb.velocity = velocity;
        }

        // centrifugal force
        Vector3 rightTarget = Vector3.Cross(Vector3.up, Forward());
        transform.RotateAround(transform.position, Forward(), Vector3.SignedAngle(transform.right, rightTarget, Forward()) * Time.deltaTime * centrifugalForce * (rb.velocity.magnitude+1));
    }

    public Vector3 Forward() => Vector3.Cross(transform.right, Vector3.up);


    protected abstract void UpdateController();
}
