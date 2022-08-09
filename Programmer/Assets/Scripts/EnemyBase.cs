using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float Speed = 2.0f;
    public float Range = 15;
    private Trans _target;

    public EventHandler OnDisable;

    private void OnEnable()
    {
        var xCoor = UnityEngine.Random.Range(-1 * Range, Range);
        transform.position = new Vector3(xCoor, transform.position.y, transform.position.z);
        _target = new Trans(transform);
        _target.position += new Vector3(xCoor, 0, -120);
    }

    private void Update()
    {
        var step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);

        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            OnDisable?.Invoke(this, EventArgs.Empty);
            enabled = false;
        }
    }
}
