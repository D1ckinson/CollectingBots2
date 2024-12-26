using TMPro;

public class ScoreViewer
{
    private TMP_Text _text;
    private IHaveScore _scoreHolder;

    public ScoreViewer(TMP_Text text, IHaveScore scoreHolder, int score = 0)
    {
        _text = text;
        _scoreHolder = scoreHolder;

        UpdateScore(score);
    }

    public void Run() =>
        _scoreHolder.ScoreChanged += UpdateScore;

    public void Stop() =>
        _scoreHolder.ScoreChanged -= UpdateScore;

    private void UpdateScore(int score) =>
        _text.text = score.ToString();
}
