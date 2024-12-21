using System.Collections.Generic;
using UnityEngine;

public class MinedResources : MonoBehaviour
{
    private List<Resource> _minedResources = new();

    public void Add(Resource resource) =>
        _minedResources.Add(resource);

    public void Remove(Resource resource) =>
        _minedResources.Remove(resource);

    public bool IsResourceMined(Resource resource) =>
        _minedResources.Contains(resource);
}
