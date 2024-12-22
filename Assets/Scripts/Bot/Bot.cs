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

    public event Action TargetReached;

    public bool IsBusy { get; private set; }

    private void OnDisable()
    {
        if (_mover == null)
            return;

        _mover.TargetReached -= TargetReached;
    }

    public void Init()
    {
        _collector = new(_collectPoint);

        _mover = GetComponent<Mover>();
        _mover.Init();
        _mover.TargetReached += TellTargetReached;
    }

    public void ExtractResource(Resource resource, Base @base)
    {
        _mover.TargetReached += PickUp;

        _resource = resource;
        _base = @base;

        IsBusy = true;
        _mover.Move(_resource.transform.position);
    }

    public void Move(Vector3 point) =>
        _mover.Move(point);

    private void TellTargetReached() =>
        TargetReached?.Invoke();

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
