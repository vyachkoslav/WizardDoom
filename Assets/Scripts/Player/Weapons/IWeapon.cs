using UnityEngine;

namespace Player.Weapons
{
    public interface IWeapon
    {
        public int CurrentAmmo { get; }
        public int CurrentLoadedAmmo { get; }
        
        public void StartShooting();
        public void StopShooting();
        public void Reload();

        public void Show();
        public void Hide();
    }
}