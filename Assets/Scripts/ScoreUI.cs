using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _lastScore = int.MinValue;

    private void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        if (_lastScore == gm.CurrentScore) return;
        _lastScore = gm.CurrentScore;
        UpdateScore(_lastScore);
    }

    public void UpdateScore(int score)
    {
        if (_scoreText != null)
            _scoreText.text = $"Очки: {score}";
    }
}

