using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _collectPoint;

    private Mover _mover;
    private ResourceCollector _collector;
    private Resource _resource;
    private Base _base;
    private Flag _flag;
    private Base _basePrefab;

    public event Action<Base> BasePlaced;
    public event Action TargetReached;
    public event Action ItemPicked;

    public bool IsBusy { get; private set; }

    public void Init()
    {
        _collector = new(_collectPoint);
        _mover = GetComponent<Mover>();
        _mover.Init();
    }

    public void ExtractResource(Resource resource, Base @base)
    {
        _mover.TargetReached += PickUp;

        _resource = resource;
        _base = @base;

        IsBusy = true;
        _mover.Move(_resource.transform.position);
    }

    public void BuildBase(Flag flag, Base basePrefab)
    {
        _flag = flag;
        _basePrefab = basePrefab;

        _mover.Move(_flag.transform.position);
        _mover.TargetReached += PlaceBase;
    }

    public void PlaceBase()
    {
        Base @base = Instantiate(_basePrefab, _flag.transform.position, Quaternion.identity);

        Destroy(_flag.gameObject);
        _mover.TargetReached -= PlaceBase;

        BasePlaced?.Invoke(@base);
        @base.TakeBot(this);
    }

    public void DropResource() =>
        _collector.Relieve();

    private void TellTargetReached()
    {
        _mover.TargetReached -= TellTargetReached;
        TargetReached?.Invoke();
    }

    private void PickUp()
    {
        _mover.TargetReached -= PickUp;
        _collector.ItemPicked += Return;

        _collector.PickUp(_resource);
    }

    private void Return()
    {
        _collector.ItemPicked -= Return;
        _mover.TargetReached += GiveResource;

        _mover.Move(_base.transform.position);
    }

    private void GiveResource()
    {
        _base.TakeResource(_collector.Relieve());
        _resource = null;
        IsBusy = false;

        _mover.TargetReached -= GiveResource;
    }
}
