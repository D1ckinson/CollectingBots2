using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ResourceSpawner _spawner;
    [SerializeField] private Base _base;
    [SerializeField] private int _startBotsCount = 3;
    [SerializeField] private MinedResources _minedStatus;

    private void Start()
    {
        _spawner.Init();
        _spawner.Run();

        _base.Init(_minedStatus);

        for (int i = 0; i < _startBotsCount; i++)
            _base.BuildBot();

        _base.RunScanner();
        _base.StartExtraction();
        _base.BuildBots();
    }
}
