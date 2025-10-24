using System.Collections.Generic;
using Player.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private RecoilController recoilController;
        [SerializeField] private Transform weaponParent;
        [SerializeField] private new Transform camera;
    
        [SerializeField] private List<Weapon> availableWeapons;

        [SerializeField] private InputActionReference shootAction;
        [SerializeField] private InputActionReference reloadAction;
    
        [SerializeField] private InputActionReference selectFirstWeapon;
        [SerializeField] private InputActionReference selectSecondWeapon;
        [SerializeField] private InputActionReference selectThirdWeapon;

        private int currentWeaponIndex = -1;
        public Weapon CurrentWeapon => availableWeapons[currentWeaponIndex];

        private void Awake()
        {
            availableWeapons.ForEach(x => x.Spawn(weaponParent, camera, recoilController));
            SelectWeapon(0);
        }
        private void OnEnable()
        {
            shootAction.action.performed += StartShooting;
            shootAction.action.canceled += StopShooting;
            reloadAction.action.performed += Reload;
            selectFirstWeapon.action.performed += SelectFirstWeapon;
            selectSecondWeapon.action.performed += SelectSecondWeapon;
            selectThirdWeapon.action.performed += SelectThirdWeapon;
        }

        private void OnDisable()
        {
            shootAction.action.performed -= StartShooting;
            shootAction.action.canceled -= StopShooting;
            reloadAction.action.performed -= Reload;
            selectFirstWeapon.action.performed -= SelectFirstWeapon;
            selectSecondWeapon.action.performed -= SelectSecondWeapon;
            selectThirdWeapon.action.performed -= SelectThirdWeapon;
        }

        private void StartShooting(InputAction.CallbackContext _)
        {
            CurrentWeapon.StartShooting();
        }

        private void StopShooting(InputAction.CallbackContext _)
        {
            CurrentWeapon.StopShooting();
        }

        private void Reload(InputAction.CallbackContext _)
        {
            CurrentWeapon.Reload();
        }

        private void SelectWeapon(int index)
        {
            if (currentWeaponIndex == index) return;
            if (index >= availableWeapons.Count) return;
        
            if (currentWeaponIndex >= 0)
                CurrentWeapon.Hide();
        
            currentWeaponIndex = index;
            CurrentWeapon.Show();
        }

        private void SelectFirstWeapon(InputAction.CallbackContext _) => SelectWeapon(0);
        private void SelectSecondWeapon(InputAction.CallbackContext _) => SelectWeapon(1);
        private void SelectThirdWeapon(InputAction.CallbackContext _) => SelectWeapon(2);
    }
}
