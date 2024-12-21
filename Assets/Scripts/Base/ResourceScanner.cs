using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private float _radius = 100f;

    private Coroutine _coroutine;

    public event Action<IEnumerable<Resource>> Searched;

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0,0,255,0.3f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void Run() =>
        _coroutine = StartCoroutine(Search());

    public void Stop()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private IEnumerator Search()
    {
        WaitForSeconds wait = new(_delay);

        while (true)
        {
            yield return wait;

            IEnumerable<Resource> resources = Physics.OverlapSphere(transform.position, _radius)
                .Select(collider => collider.GetComponent<Resource>())
                .Where(resource => resource != null);

            Searched?.Invoke(resources);
        }
    }
}
