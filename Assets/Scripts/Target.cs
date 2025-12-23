using UnityEngine;
using System;

public class Target : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _pauseDuration = 1f;
    [SerializeField] private MovementStrategyType _strategyType = MovementStrategyType.LinearPingPong;
    [SerializeField] private float _reopenDelay = 1.5f;
    [SerializeField] private float _resumeAfterOpenDelay = 0.5f;
    [SerializeField] private bool _moveOnStart = true;
    [SerializeField] private int _scoreValue = 1;
    
    private IMovementStrategy _movementStrategy;
    private bool _isActive = true;
    private Coroutine _reopenCoroutine;
    private bool _isOpen = false;
    
    private static readonly int CloseTrigger = Animator.StringToHash("Close");
    private static readonly int OpenTrigger = Animator.StringToHash("Open");
    
    public enum MovementStrategyType
    {
        LinearPingPong,
        Smooth,
        SineWave,
        RandomPause
    }
    
    private void Start()
    {
        ValidateReferences();
        InitializeMovementStrategy();

        if (_moveOnStart)
            Open();
        else
            SetActive(false);
    }
    
    private void Update()
    {
        if (!_isActive || _movementStrategy == null) return;
        _movementStrategy.UpdateMovement(Time.deltaTime);
    }
    
    private void ValidateReferences()
    {
        if (_startPoint == null || _endPoint == null)
        {
            Debug.LogError($"{gameObject.name}: Start и End точки не назначены!");
            enabled = false;
        }
        
        if (_animator == null)
            _animator = GetComponent<Animator>();
    }
    
    private void InitializeMovementStrategy()
    {
        switch (_strategyType)
        {
            case MovementStrategyType.LinearPingPong:
                _movementStrategy = new LinearPingPongStrategy(transform, _startPoint, _endPoint, _moveSpeed, _pauseDuration);
                break;
                
            case MovementStrategyType.Smooth:
                _movementStrategy = new SmoothMovementStrategy(transform, _startPoint, _endPoint, _moveSpeed, _pauseDuration);
                break;
                
            case MovementStrategyType.SineWave:
                _movementStrategy = new SineWaveMovementStrategy(transform, _startPoint, _endPoint, _moveSpeed, _pauseDuration);
                break;
                
            case MovementStrategyType.RandomPause:
                _movementStrategy = new RandomPauseMovementStrategy(transform, _startPoint, _endPoint, _moveSpeed, _pauseDuration);
                break;
                
            default:
                _movementStrategy = new LinearPingPongStrategy(transform, _startPoint, _endPoint, _moveSpeed, _pauseDuration);
                break;
        }
    }
    
    /// <summary>
    /// Метод вызывается при попадании в мишень
    /// </summary>
    public void OnHit()
    {
        // Реагируем только когда мишень в процессе открытия/открыта
        if (!_isOpen) return;

        Close();
        RestartReopenRoutine();
        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(_scoreValue);
    }
    
    /// <summary>
    /// Остановить/возобновить движение мишени
    /// </summary>
    public void SetActive(bool isActive)
    {
        // Не запускаем движение, если игра завершена
        if (isActive && GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        _isActive = isActive;
        _movementStrategy?.OnActivationChanged(isActive);
    }
    
    /// <summary>
    /// Сменить стратегию движения во время выполнения
    /// </summary>
    public void ChangeMovementStrategy(IMovementStrategy newStrategy)
    {
        _movementStrategy = newStrategy;
    }
    
    /// <summary>
    /// Сменить стратегию движения по типу
    /// </summary>
    public void ChangeMovementStrategy(MovementStrategyType newType)
    {
        _strategyType = newType;
        InitializeMovementStrategy();
    }
    
    public void StopMovementTemporarily(float duration)
    {
        StartCoroutine(StopMovementCoroutine(duration));
    }
    
    private System.Collections.IEnumerator StopMovementCoroutine(float duration)
    {
        bool wasActive = _isActive;
        SetActive(false);
        yield return new WaitForSeconds(duration);
        SetActive(wasActive);
    }

    public void Open()
    {
        if (_reopenCoroutine != null)
        {
            StopCoroutine(_reopenCoroutine);
            _reopenCoroutine = null;
        }

        _reopenCoroutine = StartCoroutine(OpenRoutine());
    }

    public void Close()
    {
        if (_reopenCoroutine != null)
        {
            StopCoroutine(_reopenCoroutine);
            _reopenCoroutine = null;
        }

        SetActive(false);

        if (_animator != null)
            _animator.SetTrigger(CloseTrigger);

        _isOpen = false;
    }

    private void RestartReopenRoutine()
    {
        if (_reopenCoroutine != null)
            StopCoroutine(_reopenCoroutine);

        _reopenCoroutine = StartCoroutine(ReopenAfterDelay());
    }

    private System.Collections.IEnumerator ReopenAfterDelay()
    {
        yield return new WaitForSeconds(_reopenDelay);
        yield return OpenRoutine();
        _reopenCoroutine = null;
    }

    private System.Collections.IEnumerator OpenRoutine()
    {
        if (_animator != null)
            _animator.SetTrigger(OpenTrigger);

        _isOpen = true;

        if (_resumeAfterOpenDelay > 0f)
            yield return new WaitForSeconds(_resumeAfterOpenDelay);

        SetActive(true);
    }
    
    public interface IMovementStrategy
    {
        void UpdateMovement(float deltaTime);
        void OnActivationChanged(bool isActive);
    }
}