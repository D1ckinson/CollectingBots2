using System;
using System.Collections.Generic;
using System.Linq;

public class Pool<T> where T : IPoolableObject<T>
{
    private Func<T> _createFunc;
    private Queue<T> _items = new();

    public Pool(Func<T> createFunc, int preloadCount = 1)
    {
        _createFunc = createFunc;

        for (int i = 0; i < preloadCount; i++)
        {
            T item = _createFunc.Invoke();

            item.Disable();
            _items.Enqueue(item);
        }
    }

    public T Get()
    {
        T item = _items.Any() ? _items.Dequeue() : _createFunc.Invoke();

        item.Enable();
        item.Disabled += Return;

        return item;
    }

    private void Return(T item)
    {
        item.Disabled -= Return;

        _items.Enqueue(item);
    }
}
