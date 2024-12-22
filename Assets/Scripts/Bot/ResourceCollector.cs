using System;
using UnityEngine;

public class ResourceCollector
{
    private Transform _itemPoint;
    private Resource _resource;

    public ResourceCollector(Transform itemPoint)
    {
        _itemPoint = itemPoint;
    }

    public event Action ItemPicked;

    public void PickUp(Resource resource)
    {
        _resource = resource;
        _resource.transform.SetParent(_itemPoint);
        _resource.transform.localPosition = Vector3.zero;

        ItemPicked?.Invoke();
    }

    public Resource Relieve()
    {
        Resource resource = _resource;

        _resource.transform.SetParent(null);
        _resource = null;

        return resource;
    }
}
