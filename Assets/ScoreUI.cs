using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void UpdateScore(int score)
    {
        scoreText.text = "Очки: " + score;
    }

}
