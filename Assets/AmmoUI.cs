using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
   

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = "" + ammo;
    }

}
