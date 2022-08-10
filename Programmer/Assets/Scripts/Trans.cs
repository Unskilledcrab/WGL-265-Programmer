using UnityEngine;

public static class TransformExtensions
{
    public static void CopyFrom(this Transform to, Transform from)
    {
        to.position = from.position;
        to.rotation = from.rotation;
        to.localScale = from.localScale;
    }
}

public class Trans
{

    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;

    public Trans(Vector3 newPosition, Quaternion newRotation, Vector3 newLocalScale)
    {
        position = newPosition;
        rotation = newRotation;
        localScale = newLocalScale;
    }

    public Trans()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        localScale = Vector3.one;
    }

    public Trans(Transform transform)
    {
        CopyFrom(transform);
    }

    public void CopyFrom(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        localScale = transform.localScale;
    }

    public void CopyTo(Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = localScale;
    }

}
