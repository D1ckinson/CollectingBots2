using System.Collections.Generic;

public interface IScanner : IBaseModule
{
    public IEnumerable<Resource> Resources { get; }

    public void Run();
}
