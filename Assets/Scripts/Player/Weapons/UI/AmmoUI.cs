using TMPro;
using UnityEngine;

namespace Player.Weapons.UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private TMP_Text currentAmmoText;
        [SerializeField] private TMP_Text reserveAmmoText;

        private void Update()
        {
            currentAmmoText.text = weaponController.CurrentWeapon.CurrentLoadedAmmo.ToString();
            reserveAmmoText.text = weaponController.CurrentWeapon.CurrentAmmo.ToString();
        }
    }
}