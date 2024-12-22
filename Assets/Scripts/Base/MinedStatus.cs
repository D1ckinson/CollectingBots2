using System.Collections.Generic;

public class MinedStatus
{
    private List<Resource> _minedResources = new();

    public void Add(Resource resource) =>
        _minedResources.Add(resource);

    public void Remove(Resource resource) =>
        _minedResources.Remove(resource);

    public bool IsResourceMined(Resource resource) =>
        _minedResources.Contains(resource);
}
