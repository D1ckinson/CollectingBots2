using System;
using System.Collections;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private Camera _camera;
    private Terrain _terrain;
    private Plane _ground = new(Vector3.up, Vector3.zero);

    public event Action Placed;

    public void Run()
    {
        _camera = Camera.main;
        _terrain = Terrain.activeTerrain;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        bool isRun = true;
        Ray ray;
        Vector3 worldPosition;

        while (isRun)
        {
            yield return null;

            ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (_ground.Raycast(ray, out float position) == false)
                continue;

            worldPosition = ray.GetPoint(position);

            worldPosition.x = Mathf.Clamp(worldPosition.x, 0, _terrain.terrainData.size.x);
            worldPosition.z = Mathf.Clamp(worldPosition.z, 0, _terrain.terrainData.size.z);

            transform.position = worldPosition;

            if (Input.GetMouseButtonDown(0))
                isRun = false;
        }

        Placed?.Invoke();
    }
}
