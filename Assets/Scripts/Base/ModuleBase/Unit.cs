using UnityEngine;

public class Unit : MonoBehaviour, IExtractUnit, IBuildUnit
{
    private Mover _mover;
    private ResourceCollector _collector;
    private ExtractInfo _extractInfo;

    public bool IsBusy { get; private set; }

    public void Extract(Resource resource, IResourceStorage storage)
    {
        IsBusy = true;
        _extractInfo = new(resource, storage);
        _mover.Move(_extractInfo.Resource.transform.position);
        _mover.TargetReached += PickUp;
    }

    private void PickUp()
    {
        _mover.TargetReached -= PickUp;
        _collector.PickUp(_extractInfo.Resource);
        _collector.ItemPicked += MoveToStorage;
    }

    private void MoveToStorage()
    {
        _collector.ItemPicked -= MoveToStorage;
        _mover.Move(_extractInfo.Storage.Positon);
        _mover.TargetReached += GiveResource;
    }

    private void GiveResource()
    {
        _mover.TargetReached -= GiveResource;
        _extractInfo.Storage.Add(_collector.Relieve());
        IsBusy = false;
    }

    private class ExtractInfo
    {
        public ExtractInfo(Resource resource, IResourceStorage storage)
        {
            Resource = resource;
            Storage = storage;
        }

        public Resource Resource { get; }
        public IResourceStorage Storage { get; }
    }
}
//public void Extract(Resource resource, IResourceStorage storage) =>
//    StartCoroutine(ExtractCoroutine(resource, storage));
//private IEnumerator ExtractCoroutine(Resource resource, IResourceStorage storage)
//{
//    _mover.Move(resource.transform.position);

//    yield return new WaitUntil(() => true);

//    _collector.PickUp(resource);

//    yield return new WaitWhile(() => true);

//    _mover.Move(storage.Positon);

//    yield return new WaitWhile(() => true);

//    storage.Add(_collector.Relieve());
//}