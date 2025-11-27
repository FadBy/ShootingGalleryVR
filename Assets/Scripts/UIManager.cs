using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public GameObject endScreen;
    public TextMeshProUGUI endScoreText;

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = "Патроны: " + ammo;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Очки: " + score;
    }

    public void ShowEndScreen(int finalScore)
    {
        endScreen.SetActive(true);
        endScoreText.text = "Игра окончена!\nОчки: " + finalScore;
    }
}
