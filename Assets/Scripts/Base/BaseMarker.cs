using System;
using UnityEngine;

public class BaseMarker : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    private Flag _flag;

    public bool IsRunning { get; private set; }
    public bool IsFlagPlaced { get; private set; }

    public event Action<Flag> FlagPlaced;

    public void Run()
    {
        if (_flag != null)
            Destroy(_flag.gameObject);

        _flag = Instantiate(_flagPrefab);
        _flag.Run();
        _flag.Placed += TellFlagPlaced;
        IsRunning = true;
    }

    private void TellFlagPlaced()
    {
        _flag.Placed -= TellFlagPlaced;
        FlagPlaced?.Invoke(_flag);
        IsFlagPlaced = true;
        IsRunning = false;
    }
}
