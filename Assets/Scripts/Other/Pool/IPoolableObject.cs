using System;

public interface IPoolableObject<T>
{
    public event Action<T> Disabled;

    public void Enable();

    public void Disable();
}