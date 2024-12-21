using TMPro;

public class ScoreViewer
{
    private TMP_Text _text;
    private Base _base;

    public ScoreViewer(TMP_Text text, Base @base,int score=0)
    {
        _text = text;
        _base = @base;

        UpdateScore(score);
    }

    public void Run() =>
        _base.ScoreChange += UpdateScore;

    public void Stop() =>
        _base.ScoreChange -= UpdateScore;

    private void UpdateScore(int score) =>
        _text.text = score.ToString();
}
