using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Collections;

public class ScannerModule : MonoBehaviour, IScanner
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private float _radius = 100f;

    public IEnumerable<Resource> Resources { get => Resources.ToList(); private set { Resources = value; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = new(0, 0, 255, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void Run() => 
        StartCoroutine(Search());

    private IEnumerator Search()
    {
        WaitForSeconds wait = new(_delay);

        while (true)
        {
            yield return wait;

            Resources = Physics.OverlapSphere(transform.position, _radius)
                .Select(collider => collider.GetComponent<Resource>())
                .Where(resource => resource != null);
        }
    }
}
