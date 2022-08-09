using System;
using UnityEngine;

public static class Constants
{
    public static int ZBoundary = -120;
}

public class EnemyBase : MonoBehaviour
{
    public float Speed = 2.0f;
    public float Range = 15;
    public EventHandler OnDisable;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        var xCoor = UnityEngine.Random.Range(-1 * Range, Range);
        transform.position = new Vector3(xCoor, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + (Vector3.back * Speed * Time.deltaTime));
    }

    private void Update()
    {
        if (Math.Abs(transform.position.z - Constants.ZBoundary) < 0.1f)
        {
            OnDisable?.Invoke(this, EventArgs.Empty);
            gameObject.SetActive(false);
        }
    }
}
