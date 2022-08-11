using System;
using UnityEngine;

public static class Constants
{
    public static int ZBoundary = -10;
}

public class EnemyBase : Obstacle
{
    public float Speed = 2.0f;
    public float Range = 15;
    public EventHandler OnDisable;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected override void Enable()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        var xCoor = UnityEngine.Random.Range(-1 * Range, Range);
        transform.position = new Vector3(xCoor, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + (Vector3.back * Speed * Time.deltaTime));
    }
}
