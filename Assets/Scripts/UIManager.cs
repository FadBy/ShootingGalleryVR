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
        if (ammoText != null)
            ammoText.text = $"Боеприпасы: {ammo}";
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Очки: {score}";
    }

    public void ShowEndScreen(int finalScore)
    {
        if (endScreen != null)
            endScreen.SetActive(true);
        if (endScoreText != null)
            endScoreText.text = $"Игра окончена!\nОчки: {finalScore}";
    }

    public void HideEndScreen()
    {
        if (endScreen != null)
            endScreen.SetActive(false);
    }
}
