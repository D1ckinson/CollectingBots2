using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private Base _base;
    [SerializeField] private int _startBotsCount = 3;
    [SerializeField] private BotFactory _botFactory;
    [SerializeField] private BaseFactory _baseFactory;

    private void Start()
    {
        _spawner.Init();
        _spawner.Run();

        new BaseUtilities(_botFactory, _baseFactory).InitBase(_base);

        for (int i = 0; i < _startBotsCount; i++)
            _base.BuildBot();
    }
}
