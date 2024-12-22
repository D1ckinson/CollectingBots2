using UnityEngine;

public class BaseUtilities
{
    private BotFactory _botFactory;
    private BaseFactory _baseFactory;
    private MinedStatus _minedStatus = new();

    public BaseUtilities(BotFactory botFactory, BaseFactory baseFactory)
    {
        _botFactory = botFactory;
        _baseFactory = baseFactory;
    }

    public int BotCost { get; } = 3;
    public int BaseCost { get; } = 5;
    public float ExtractDelay { get; } = 0.1f;

    public void AddMinedResource(Resource resource) =>
        _minedStatus.Add(resource);

    public Base CreateBase(Vector3 position) =>
        _baseFactory.Create(position);

    public Bot CreateBot(Vector3 position) =>
        _botFactory.Create(position);

    public void InitBase(Base @base)
    {
        @base.Init(this);
        @base.RunScanner();
        @base.StartExtraction();
        @base.BuildBots();
    }

    public bool IsResourceMined(Resource resource) =>
        _minedStatus.IsResourceMined(resource);

    public void RemoveMinedResource(Resource resource) =>
        _minedStatus.Remove(resource);
}
