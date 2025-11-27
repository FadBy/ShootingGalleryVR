using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    public GameObject endScreen;
    public TextMeshProUGUI endScoreText;

    public void ShowEndScreen(int finalScore)
    {
        endScreen.SetActive(true);
        endScoreText.text = "Игра окончена!\nОчки: " + finalScore;
    }
}
