using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Force = 40f;
    private ParticleSystem _ps;
    private float _radius = 20f;
    private List<Rigidbody> _rbsInRange = new();

    private void Start()
    {
        var rb = GetComponent<SphereCollider>();
        _ps = GetComponent<ParticleSystem>();
        _radius = rb.radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (other.GetComponent<Player>() == null && rb != null)
            _rbsInRange.Add(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
            _rbsInRange.Remove(rb);
    }

    public void Explode()
    {
        _ps.Play();
        foreach (var rb in _rbsInRange)
        {
            rb.AddExplosionForce(Force, transform.position, _radius);
        }
    }
}
