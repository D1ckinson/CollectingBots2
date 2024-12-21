using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private float _delay = 1f;
    [SerializeField] private float _radius = 30f;

    private Pool<Resource> _resources;
    private Vector2 _center;

    private void OnDrawGizmos()
    {
        Gizmos.color = new(255, 255, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    public void Init()
    {
        _resources = new(() => Instantiate(_resourcePrefab));
        _center = new(transform.position.x, transform.position.z);
    }

    public void Run() =>
        StartCoroutine(Spawn());

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new(_delay);

        while (true)
        {
            yield return wait;

            Resource resource = _resources.Get();
            resource.transform.position = GetSpawnPoint();
        }
    }

    private Vector3 GetSpawnPoint()
    {
        Vector2 randomPoint2D = Random.insideUnitCircle * _radius;
        Vector3 offset = new(randomPoint2D.x, transform.position.y, randomPoint2D.y);
        Vector3 spawnPoint = transform.position + offset;

        return spawnPoint;
    }
}
