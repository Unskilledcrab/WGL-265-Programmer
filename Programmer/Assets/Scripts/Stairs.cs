using UnityEngine;

public class Stairs : Obstacle
{
    protected override float ZBoundary() => -100;
    public float Speed = 10.0f;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected override void OnSpawn(Transform spawnPoint)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, spawnPoint.position.z);
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + (Vector3.back * Speed * Time.deltaTime));
    }
}
