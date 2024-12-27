using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitControllerModule : MonoBehaviour, IBaseModule
{
    private List<Unit> _units = new();

    public void Build() { }

    public void Extract(IScanner scanner, IResourceStorage storage) =>
        StartCoroutine(ExtractCoroutine(scanner, storage));

    private IEnumerator ExtractCoroutine(IScanner scanner, IResourceStorage storage)
    {
        while (true)
        {
            yield return null;

            IEnumerable<Resource> resources = scanner.Resources;

            for (int i = 0; i < resources.Count(); i++)
            {
                if (TryGetFreeUnit(out Unit unit) == false)
                    break;

                unit.Extract(resources.ElementAt(i), storage);
            }
        }
    }

    public bool TryGetFreeUnit(out Unit unit)
    {
        unit = _units.FirstOrDefault(unit => unit.IsBusy == false);

        return unit != null;
    }
}
