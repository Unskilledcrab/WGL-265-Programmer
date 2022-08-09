using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 2.0f;

    private Trans _target;

    private void Awake()
    {
        _target = new Trans(transform);
    }

    private void Update()
    {
        var step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }

    public void Left()
    {
        _target.position = _target.position + Vector3.left;
    }
    public void Right()
    {
        _target.position = _target.position + Vector3.right;
    }
    public void Up()
    {
        _target.position = _target.position + Vector3.up;
    }
    public void Down()
    {
        _target.position = _target.position + Vector3.down;
    }

    public void Jump()
    {
        _target.position = _target.position + Vector3.up;
    }
}
