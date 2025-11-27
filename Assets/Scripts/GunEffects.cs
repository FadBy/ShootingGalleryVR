using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunEffects : MonoBehaviour
{
    public BulletTrail _bulletTrailPrefab;
    public List<VisualEffect> _effects;

    private GunShoot _gunShoot;

    private void Awake()
    {
        _gunShoot = GetComponent<GunShoot>();
    }

    private void OnEnable()
    {
        _gunShoot.ShootCallback += OnShoot;
    }

    private void OnDisable()
    {
        _gunShoot.ShootCallback -= OnShoot;
    }

    private void OnShoot()
    {
        foreach (var effect in _effects)
        {
            effect.Play();
        }
        
        var bulletTrail = Instantiate(_bulletTrailPrefab, _gunShoot.muzzle.transform.position, _gunShoot.transform.rotation);
        if (_gunShoot.DidHit)
        {
            bulletTrail.EndPoint = _gunShoot.HitPoint.point;
        }
        else
        {
            bulletTrail.EndPoint = bulletTrail.transform.position + _gunShoot.muzzle.forward * 100;
            
        }
    }
}
