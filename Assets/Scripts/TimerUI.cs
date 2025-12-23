using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private int _lastDisplayedSeconds = -1;

    private void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        int secondsTotal = Mathf.FloorToInt(Mathf.Max(0f, gm.TimeLeft));
        if (secondsTotal == _lastDisplayedSeconds) return;

        _lastDisplayedSeconds = secondsTotal;
        UpdateTime(secondsTotal);
    }

    public void UpdateTime(float secondsLeft)
    {
        if (_timerText == null) return;

        secondsLeft = Mathf.Max(0f, secondsLeft);
        int minutes = Mathf.FloorToInt(secondsLeft / 60f);
        int seconds = Mathf.FloorToInt(secondsLeft % 60f);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
}

