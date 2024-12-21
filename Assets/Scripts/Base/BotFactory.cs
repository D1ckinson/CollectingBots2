using UnityEngine;

public class BotFactory : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private Transform _spawn;

    public Bot Create() =>
        Instantiate(_botPrefab, _spawn.position, Quaternion.identity);
}
