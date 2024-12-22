using UnityEngine;

public class BotFactory : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot Create(Vector3 position) =>
        Instantiate(_botPrefab, position, Quaternion.identity);
}
