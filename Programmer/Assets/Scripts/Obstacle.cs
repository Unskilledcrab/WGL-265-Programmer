using System;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected virtual float ZBoundary() => -10;
    public void Spawn(Transform spawnPoint)
    {
        OnSpawn(spawnPoint);
        gameObject.SetActive(true);
    }
    protected abstract void OnSpawn(Transform spawnPoint);
    protected virtual void OnUpdate() { }
    private void Update()
    {
        OnUpdate();
        if (transform.position.z < ZBoundary())
        {
            this.ReturnToPool();
        }
    }
}
