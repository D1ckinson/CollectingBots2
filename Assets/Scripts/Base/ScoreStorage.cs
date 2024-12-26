using System;

public class ScoreStorage
{
    private int _count;

    public event Action<int> ScoreChanged;

    public void Add(Resource resource)
    {
        resource.Disable();
        _count++;

        ScoreChanged?.Invoke(_count);
    }

    public bool TrySpend(int count)
    {
        if (_count < count)
            return false;

        _count -= count;

        ScoreChanged?.Invoke(_count);

        return true;
    }
}
