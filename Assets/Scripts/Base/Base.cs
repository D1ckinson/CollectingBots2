using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ResourceScanner))]
[RequireComponent(typeof(BaseMarker))]
public class Base : MonoBehaviour, IHaveScore//ResourceStorage// MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _botSpawn;

    private ResourceScanner _scanner;
    private ScoreViewer _scoreViewer;
    private BaseMarker _baseMarker;
    private HashSet<Resource> _resources = new();
    //private int _score = 0;
    private ScoreStorage _scoreStorage = new();
    private Coroutine _spendScore;
    private Coroutine _extract;
    private List<Bot> _bots = new();
    private bool _isAlreadyBuilt = false;
    private BaseUtilities _utilities;

    public event Action<int> ScoreChanged;

    //public event Action<int> ScoreChange;

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

    public void Init(BaseUtilities utilities)
    {
        _utilities = utilities;
        _scanner = GetComponent<ResourceScanner>();
        _baseMarker = GetComponent<BaseMarker>();

        _scoreStorage.ScoreChanged += TellScoreChanged;
        _scoreViewer = new(_text, this);//_scoreViewer = new(_text, this, _score);
        _scoreViewer.Run();
    }

    public void TellScoreChanged(int score) =>
        ScoreChanged?.Invoke(score);

    public void StartExtraction() =>
        _extract = StartCoroutine(ExtractResources());

    public void BuildBot()
    {
        Bot bot = _utilities.CreateBot(_botSpawn.position);

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
        _utilities.RemoveMinedResource(resource);
        //resource.Disable();

        _scoreStorage.Add(resource);
        //_score++;
        //ScoreChange?.Invoke(_score);
    }

    private void StopExtraction()
    {
        if (_extract == null)
            return;

        StopCoroutine(_extract);
    }

    private IEnumerator ExtractResources()
    {
        WaitForSeconds wait = new(_utilities.ExtractDelay);

        while (true)
        {
            yield return wait;

            _resources.RemoveWhere(resource => resource.isActiveAndEnabled == false);

            for (int i = 0; i < _resources.Count(); i++)
            {
                if (TryGetFreeBot(out Bot bot) == false)
                    break;

                Resource resource = _resources.ElementAt(i);

                if (_utilities.IsResourceMined(resource))
                    continue;

                _utilities.AddMinedResource(resource);
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
        //yield return new WaitUntil(() => _score >= _utilities.BaseCost);
        yield return new WaitUntil(() => _scoreStorage.TrySpend(_utilities.BaseCost));

        StopExtraction();
        Bot bot;

        while (TryGetFreeBot(out bot) == false)
            yield return null;

        _isAlreadyBuilt = true;
        //_score -= _utilities.BaseCost;

        bool isTargetReached = false;
        void SetTrue() => isTargetReached = true;

        bot.Move(flag.transform.position);
        bot.TargetReached += SetTrue;

        yield return new WaitUntil(() => isTargetReached);

        bot.TargetReached -= SetTrue;

        Base @base = _utilities.CreateBase(flag.transform.position);
        _utilities.InitBase(@base);
        @base.TakeBot(bot);

        _bots.Remove(bot);

        Destroy(flag.gameObject);
        BuildBots();
        StartExtraction();
    }

    private IEnumerator BuildBotsCoroutine()
    {
        while (true)
        {
            yield return null;

            //if (_score < _utilities.BotCost)
            //    continue;

            if (_scoreStorage.TrySpend(_utilities.BotCost) == false)
                continue;

            //_score -= _utilities.BotCost;
            //ScoreChange?.Invoke(_score);

            BuildBot();
        }
    }

    private void GetScanResults(IEnumerable<Resource> resources) =>
        _resources.UnionWith(resources);
}
