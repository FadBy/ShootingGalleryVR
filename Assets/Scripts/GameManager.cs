using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Settings")]
    public int maxAmmo = 10;
    public bool useAmmoLimit = true;
    public float roundDuration = 60f;
    public float _switchModeTime = 15f;
    public Target.MovementStrategyType[] movementModes = new Target.MovementStrategyType[]
    {
        Target.MovementStrategyType.LinearPingPong,
        Target.MovementStrategyType.Smooth,
        Target.MovementStrategyType.SineWave
    };

    [Header("Runtime State")]
    public int currentAmmo;
    public int score = 0;
    public int CurrentScore => score;
    public float TimeLeft => _timeLeft;
    public bool IsGameOver => gameOver;

    [Header("References")]
    public UIManager uiManager;
    public AmmoUI ammoUI;
    public Target[] targets;

    private bool gameOver = true;
    private float _timeLeft;
    private float _modeTimer;
    private int _currentModeIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CacheTargetsIfNeeded();
        RestartGame();
    }

    public void UseAmmo()
    {
        if (gameOver) return;

        if (!useAmmoLimit)
            return;

        currentAmmo--;
        uiManager.UpdateAmmo(currentAmmo);
        ammoUI.UpdateAmmo(currentAmmo);
    }

    public void AddScore(int amount)
    {
        if (gameOver) return;

        score += amount;
        uiManager.UpdateScore(score);
    }

    private void EndGame()
    {
        gameOver = true;
        StopTargets();
        uiManager.ShowEndScreen(score);
        Debug.Log("Игра окончена! Итоговый счёт: " + score);
    }

    public void RestartGame()
    {
        gameOver = false;
        score = 0;
        currentAmmo = maxAmmo;
        _timeLeft = roundDuration;
        _modeTimer = _switchModeTime;
        _currentModeIndex = 0;

        uiManager.UpdateAmmo(currentAmmo);
        ammoUI.UpdateAmmo(currentAmmo);
        uiManager.UpdateScore(score);

        uiManager.HideEndScreen();

        CacheTargetsIfNeeded();
        ApplyModeToTargets(GetCurrentMode());
        OpenTargets();
    }

    private void Update()
    {
        if (gameOver) return;

        _timeLeft -= Time.deltaTime;
        if (_timeLeft <= 0f)
        {
            _timeLeft = 0f;
            EndGame();
            return;
        }

        _modeTimer -= Time.deltaTime;
        if (_modeTimer <= 0f && movementModes != null && movementModes.Length > 0)
        {
            AdvanceMode();
            _modeTimer = _switchModeTime;
        }
    }

    private void AdvanceMode()
    {
        if (movementModes == null || movementModes.Length == 0) return;
        _currentModeIndex = (_currentModeIndex + 1) % movementModes.Length;
        ApplyModeToTargets(GetCurrentMode());
    }

    private Target.MovementStrategyType GetCurrentMode()
    {
        if (movementModes == null || movementModes.Length == 0)
            return Target.MovementStrategyType.LinearPingPong;

        return movementModes[Mathf.Clamp(_currentModeIndex, 0, movementModes.Length - 1)];
    }

    private void ApplyModeToTargets(Target.MovementStrategyType mode)
    {
        if (targets == null) return;
        foreach (var target in targets)
        {
            if (target != null)
                target.ChangeMovementStrategy(mode);
        }
    }

    private void OpenTargets()
    {
        if (targets == null) return;
        foreach (var target in targets)
        {
            if (target != null)
                target.Open();
        }
    }

    private void StopTargets()
    {
        if (targets == null) return;
        foreach (var target in targets)
        {
            if (target != null)
                target.SetActive(false);
        }
    }

    private void CacheTargetsIfNeeded()
    {
        if (targets == null || targets.Length == 0)
        {
            targets = FindObjectsOfType<Target>();
        }
    }
}
