using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 400.0f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Left()
    {
        _rb.AddForce(Vector3.left * Speed);
    }
    public void Right()
    {
        _rb.AddForce(Vector3.right * Speed);
    }
    public void Up()
    {
        _rb.AddForce(Vector3.up * Speed);
    }
    public void Down()
    {
        _rb.AddForce(Vector3.down * Speed);
    }
}
