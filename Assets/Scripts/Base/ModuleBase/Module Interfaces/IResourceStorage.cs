using System.Numerics;

public interface IResourceStorage : IBaseModule
{
    public int Count { get; }
    public UnityEngine.Vector3 Positon { get; }

    public void Add(Resource resource);

    public void Spend(int value);
}
