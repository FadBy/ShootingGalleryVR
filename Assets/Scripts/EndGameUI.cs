using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void ShowEndScreen(int finalScore)
    {
        if (_panel != null)
            _panel.SetActive(true);
        if (_scoreText != null)
            _scoreText.text = $"Игра окончена!\nОчки: {finalScore}";
    }

    public void HideEndScreen()
    {
        if (_panel != null)
            _panel.SetActive(false);
    }
}


