using UnityEngine;
using UnityEngine.InputSystem;

public class GunShoot : MonoBehaviour
{
    [Header("Gun Setup")]
    public Transform muzzle;
    public float shootDistance = 100f;
    public float hitForce = 10f;
    public GameObject hitEffectPrefab;

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
        if (GameManager.Instance == null) return;

        // проверка на патроны
        if (GameManager.Instance.currentAmmo <= 0)
        {
            Debug.Log("❌ Патроны закончились!");
            return;
        }

        GameManager.Instance.UseAmmo();

        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, shootDistance))
        {
            Debug.Log("🎯 Попадание в: " + hit.collider.name);

            if (hitEffectPrefab)
                Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            // Проверяем, есть ли скрипт Target
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                target.Hit();
            }

            if (hit.rigidbody)
                hit.rigidbody.AddForce(-hit.normal * hitForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("💨 Мимо. Луч никуда не попал.");
        }
    }
}
