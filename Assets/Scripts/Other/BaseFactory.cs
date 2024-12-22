using UnityEngine;

public class BaseFactory : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public Base Create(Vector3 position) =>
        Instantiate(_basePrefab, position, Quaternion.identity);
}
