using System;
using UnityEngine;

public class ResourceStorageModule : MonoBehaviour, IResourceStorage
{
    private int _count;

    public int Count => _count;

    public Vector3 Positon => transform.position;

    public void Add(Resource resource) =>
        _count += 1;

    public void Spend(int value)
    {
        if (value > _count)
            throw new ArgumentOutOfRangeException(nameof(value));

        _count -= value;
    }
}
