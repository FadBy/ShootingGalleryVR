using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class GunEffects : MonoBehaviour
{
    public List<VisualEffect> _effects;
    
    [Header("Input")]
    public InputActionReference shootAction;

    private void OnEnable()
    {
        if (shootAction != null)
            shootAction.action.performed += OnShoot;
    }

    private void OnDisable()
    {
        if (shootAction != null)
            shootAction.action.performed -= OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.currentAmmo <= 0) return;
        foreach (var effect in _effects)
        {
            effect.Play();
        }
    }
}
