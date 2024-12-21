using System;
using UnityEngine;

public class Resource : MonoBehaviour, IPoolableObject<Resource>
{
    public event Action<Resource> Disabled;

    public void Disable()
    {
        gameObject.SetActive(false);
        Disabled?.Invoke(this);
    }

    public void Enable() =>
        gameObject.SetActive(true);
}
