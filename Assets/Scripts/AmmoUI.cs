using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoText;

    public void UpdateAmmo(int ammo)
    {
        if (_ammoText != null)
            _ammoText.text = $"{ammo}";
    }
}


