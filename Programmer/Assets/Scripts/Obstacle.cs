using System;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected abstract void Enable();
    protected virtual void OnUpdate() { }
    private void OnEnable()
    {
        Enable();
    }
    private void Update()
    {
        OnUpdate();
        if (Math.Abs(transform.position.z - Constants.ZBoundary) < 0.1f)
        {
            this.ReturnToPool();
        }
    }
}
