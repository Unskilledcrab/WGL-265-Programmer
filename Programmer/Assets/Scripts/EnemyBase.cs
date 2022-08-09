using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float Speed = 2.0f;
    private Trans _target;

    private void OnEnable()
    {
        var xCoor = Random.Range(-5, 5);
        transform.position = new Vector3(xCoor, transform.position.y, transform.position.z);
        _target = new Trans(transform);
        _target.position += new Vector3(xCoor, 0, -300);
    }

    private void Update()
    {
        var step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
    }
}
