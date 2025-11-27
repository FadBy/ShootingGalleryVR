using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxAmmo = 10;
    public int currentAmmo;
    public int score = 0;

    [Header("UI")]
    public UIManager uiManager;
    public AmmoUI ammoUI;
    public ScoreUI scoreUI;
    public EndGameUI endGameUI;

    private bool gameOver = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        uiManager.UpdateAmmo(currentAmmo);
        ammoUI.UpdateAmmo(currentAmmo);
        uiManager.UpdateScore(score);
        scoreUI.UpdateScore(score);
    }

    public void UseAmmo()
    {
        if (gameOver) return;

        currentAmmo--;
        uiManager.UpdateAmmo(currentAmmo);
        ammoUI.UpdateAmmo(currentAmmo);

        if (currentAmmo <= 0)
            EndGame();
    }

    public void AddScore(int amount)
    {
        if (gameOver) return;

        score += amount;
        uiManager.UpdateScore(score);
        scoreUI.UpdateScore(score);
    }

    private void EndGame()
    {
        gameOver = true;
        uiManager.ShowEndScreen(score);
        endGameUI.ShowEndScreen(score);
        Debug.Log("🏁 Игра окончена! Итоговый счёт: " + score);
    }
}
