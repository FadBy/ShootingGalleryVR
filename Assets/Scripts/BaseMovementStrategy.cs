// Файл: MovementStrategies.cs
// Отдельный файл со всеми стратегиями движения

using UnityEngine;
using System;

/// <summary>
/// Базовый класс для всех стратегий движения
/// </summary>
public abstract class BaseMovementStrategy : Target.IMovementStrategy
{
    protected Transform _targetTransform;
    protected Transform _startPoint;
    protected Transform _endPoint;
    protected float _speed;
    protected float _pauseDuration;
    protected bool _isActive = true;
    
    protected BaseMovementStrategy(Transform targetTransform, Transform startPoint, 
                                 Transform endPoint, float speed, float pauseDuration)
    {
        _targetTransform = targetTransform;
        _startPoint = startPoint;
        _endPoint = endPoint;
        _speed = speed;
        _pauseDuration = pauseDuration;
    }
    
    public abstract void UpdateMovement(float deltaTime);
    
    public virtual void OnActivationChanged(bool isActive)
    {
        _isActive = isActive;
    }
    
    protected Vector3 GetStartPosition() => _startPoint.position;
    protected Vector3 GetEndPosition() => _endPoint.position;

    protected float CalculateNormalizedPositionAlongPath()
    {
        Vector3 start = GetStartPosition();
        Vector3 end = GetEndPosition();
        Vector3 path = end - start;
        float length = path.magnitude;

        if (length < Mathf.Epsilon)
            return 0f;

        float projectedLength = Vector3.Dot(_targetTransform.position - start, path.normalized);
        return Mathf.Clamp01(projectedLength / length);
    }

    protected bool ShouldMoveToEndFirst()
    {
        float toStart = Vector3.Distance(_targetTransform.position, GetStartPosition());
        float toEnd = Vector3.Distance(_targetTransform.position, GetEndPosition());
        return toStart <= toEnd;
    }
}

/// <summary>
/// Линейное движение туда-обратно с паузами (стратегия по умолчанию)
/// </summary>
public class LinearPingPongStrategy : BaseMovementStrategy
{
    private bool _movingToEnd = true;
    private float _pauseTimer = 0f;
    
    public LinearPingPongStrategy(Transform targetTransform, Transform startPoint, 
                                Transform endPoint, float speed, float pauseDuration) 
        : base(targetTransform, startPoint, endPoint, speed, pauseDuration)
    {
        _movingToEnd = ShouldMoveToEndFirst();
    }
    
    public override void UpdateMovement(float deltaTime)
    {
        if (!_isActive) return;
        
        // Если на паузе
        if (_pauseTimer > 0)
        {
            _pauseTimer -= deltaTime;
            return;
        }
        
        // Движение к цели
        Vector3 targetPosition = _movingToEnd ? GetEndPosition() : GetStartPosition();
        float step = _speed * deltaTime;
        _targetTransform.position = Vector3.MoveTowards(
            _targetTransform.position, 
            targetPosition, 
            step
        );
        
        // Проверка достижения цели
        if (Vector3.Distance(_targetTransform.position, targetPosition) < 0.01f)
        {
            // Переключаем направление и ставим на паузу
            _movingToEnd = !_movingToEnd;
            _pauseTimer = _pauseDuration;
        }
    }
}

/// <summary>
/// Плавное движение с интерполяцией
/// </summary>
public class SmoothMovementStrategy : BaseMovementStrategy
{
    private float _movementProgress = 0f;
    private bool _movingForward = true;
    
    public SmoothMovementStrategy(Transform targetTransform, Transform startPoint, 
                                Transform endPoint, float speed, float pauseDuration) 
        : base(targetTransform, startPoint, endPoint, speed, pauseDuration)
    {
        _movementProgress = CalculateNormalizedPositionAlongPath();
        _movingForward = ShouldMoveToEndFirst();
    }
    
    public override void UpdateMovement(float deltaTime)
    {
        if (!_isActive) return;
        
        // Обновляем прогресс движения
        if (_movingForward)
        {
            _movementProgress += deltaTime * _speed;
            if (_movementProgress >= 1f)
            {
                _movementProgress = 1f;
                _movingForward = false;
            }
        }
        else
        {
            _movementProgress -= deltaTime * _speed;
            if (_movementProgress <= 0f)
            {
                _movementProgress = 0f;
                _movingForward = true;
            }
        }
        
        // Применяем движение с плавной интерполяцией
        _targetTransform.position = Vector3.Lerp(
            GetStartPosition(), 
            GetEndPosition(), 
            _movementProgress
        );
    }
}

/// <summary>
/// Движение по синусоиде
/// </summary>
public class SineWaveMovementStrategy : BaseMovementStrategy
{
    private float _timeCounter = 0f;
    
    public SineWaveMovementStrategy(Transform targetTransform, Transform startPoint, 
                                  Transform endPoint, float speed, float pauseDuration) 
        : base(targetTransform, startPoint, endPoint, speed, pauseDuration)
    {
        float normalizedPosition = CalculateNormalizedPositionAlongPath();
        float sineValue = Mathf.Clamp((normalizedPosition * 2f) - 1f, -1f, 1f);
        _timeCounter = Mathf.Asin(sineValue);
    }
    
    public override void UpdateMovement(float deltaTime)
    {
        if (!_isActive) return;
        
        _timeCounter += deltaTime * _speed;
        
        // Вычисляем смещение по синусоиде
        float sineValue = Mathf.Sin(_timeCounter);
        Vector3 direction = (GetEndPosition() - GetStartPosition()).normalized;
        float distance = Vector3.Distance(GetStartPosition(), GetEndPosition());
        
        // Базовое положение + синусоидальное смещение
        float offset = (sineValue + 1f) * 0.5f * distance; // Нормализуем от 0 до 1
        _targetTransform.position = GetStartPosition() + direction * offset;
    }
}

/// <summary>
/// Линейное движение со случайными паузами
/// </summary>
public class RandomPauseMovementStrategy : BaseMovementStrategy
{
    private bool _movingToEnd = true;
    private float _pauseTimer = 0f;
    private float _randomPauseMultiplier = 1f;
    
    public RandomPauseMovementStrategy(Transform targetTransform, Transform startPoint, 
                                     Transform endPoint, float speed, float pauseDuration) 
        : base(targetTransform, startPoint, endPoint, speed, pauseDuration)
    {
        _movingToEnd = ShouldMoveToEndFirst();
        GenerateRandomPause();
    }
    
    public override void UpdateMovement(float deltaTime)
    {
        if (!_isActive) return;
        
        if (_pauseTimer > 0)
        {
            _pauseTimer -= deltaTime;
            return;
        }
        
        Vector3 targetPosition = _movingToEnd ? GetEndPosition() : GetStartPosition();
        float step = _speed * deltaTime;
        _targetTransform.position = Vector3.MoveTowards(
            _targetTransform.position, 
            targetPosition, 
            step
        );
        
        if (Vector3.Distance(_targetTransform.position, targetPosition) < 0.01f)
        {
            _movingToEnd = !_movingToEnd;
            _pauseTimer = _pauseDuration * _randomPauseMultiplier;
            GenerateRandomPause();
        }
    }
    
    private void GenerateRandomPause()
    {
        _randomPauseMultiplier = UnityEngine.Random.Range(0.5f, 2f);
    }
}