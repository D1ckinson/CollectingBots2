using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ResourceScanner))]
[RequireComponent(typeof(BotFactory))]
[RequireComponent(typeof(BaseMarker))]
public class Base : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private ResourceScanner _scanner;
    private ScoreViewer _scoreViewer;
    private BotFactory _botFactory;
    private BaseMarker _baseMarker;
    private HashSet<Resource> _resources = new();
    private MinedResources _minedStatus;
    private int _score = 0;
    private int _botCost = 3;
    private int _baseCost = 5;
    private Coroutine _spendScore;
    private Coroutine _extract;
    private float _extractDelay = 0.1f;
    private List<Bot> _bots = new();
    private bool _isAlreadyBuilt = false;

    public event Action<int> ScoreChange;

    private void OnMouseUp()
    {
        if (_bots.Count <= 1)
            return;

        if (_isAlreadyBuilt)
            return;

        if (_baseMarker.IsRunning == false && _baseMarker.IsFlagPlaced)
            BuildBots();

        _baseMarker.Run();
        _baseMarker.FlagPlaced += BuildBase;
    }

    public void Init(MinedResources minedStatus)
    {
        _scanner = GetComponent<ResourceScanner>();
        _botFactory = GetComponent<BotFactory>();
        _baseMarker = GetComponent<BaseMarker>();

        _minedStatus = minedStatus;
        _scoreViewer = new(_text, this, _score);
        _scoreViewer.Run();
    }

    public void StartExtraction()
    {
        _extract = StartCoroutine(ExtractResources());
    }

    public void BuildBot()
    {
        Bot bot = _botFactory.Create();

        bot.Init();
        _bots.Add(bot);
    }

    public void BuildBots()
    {
        if (_spendScore != null)
        {
            StopCoroutine(_spendScore);
            _spendScore = null;
        }

        _spendScore = StartCoroutine(BuildBotsCoroutine());
    }

    public void TakeBot(Bot bot) =>
        _bots.Add(bot);

    public bool TryGetFreeBot(out Bot bot)
    {
        bot = _bots.FirstOrDefault(bot => bot.IsBusy == false);

        return bot != null;
    }

    public void RunScanner()
    {
        _scanner.Searched += GetScanResults;
        _scanner.Run();
    }

    public void TakeResource(Resource resource)
    {
        _resources.Remove(resource);
        _minedStatus.Remove(resource);
        resource.Disable();

        _score++;
        ScoreChange?.Invoke(_score);
    }

    private void StopExtraction()
    {
        if (_extract == null)
            return;

        StopCoroutine(_extract);
    }

    private IEnumerator ExtractResources()
    {
        WaitForSeconds wait = new(_extractDelay);

        while (true)
        {
            yield return wait;

            for (int i = 0; i < _resources.Count(); i++)
            {
                if (TryGetFreeBot(out Bot bot) == false)
                    break;

                Resource resource = _resources.ElementAt(i);

                if (resource.isActiveAndEnabled == false)
                    continue;

                if (_minedStatus.IsResourceMined(resource))
                    continue;

                _minedStatus.Add(resource);
                bot.ExtractResource(resource, this);
            }
        }
    }

    private void BuildBase(Flag flag)
    {
        if (_spendScore != null)
        {
            StopCoroutine(_spendScore);
            _spendScore = null;
        }

        _spendScore = StartCoroutine(BuildBaseCoroutine(flag));
    }

    private IEnumerator BuildBaseCoroutine(Flag flag)
    {
        while (_score < _baseCost)
        {
            yield return null;
        }

        StopExtraction();
        Bot bot;

        while (TryGetFreeBot(out bot) == false)
        {
            yield return null;
        }

        _isAlreadyBuilt = true;
        _score -= _baseCost;
        bot.DropResource();
        bot.BuildBase(flag, this);
        bot.BasePlaced += InitBase;
        _bots.Remove(bot);

        BuildBots();
        StartExtraction();
    }

    private void InitBase(Base @base)
    {
        @base.Init(_minedStatus);
        @base.RunScanner();
        @base.StartExtraction();
        @base.BuildBots();
    }

    private IEnumerator BuildBotsCoroutine()
    {
        while (true)
        {
            yield return null;

            if (_score < _botCost)
                continue;

            _score -= _botCost;
            ScoreChange?.Invoke(_score);

            BuildBot();
        }
    }

    private void GetScanResults(IEnumerable<Resource> resources) =>
        _resources.UnionWith(resources);
}
