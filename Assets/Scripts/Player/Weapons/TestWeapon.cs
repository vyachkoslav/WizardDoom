using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player.Weapons
{
    [CreateAssetMenu(fileName = "Test Weapon", menuName = "Weapons/TestWeapon")]
    public class TestWeapon : Weapon
    {
        private readonly RaycastHit[] hits = new RaycastHit[2];
        
        private CancellationTokenSource shootingCancelSource;
        private CancellationTokenSource reloadCancelSource;

        private bool IsShooting => shootingCancelSource != null;
        private bool IsReloading => reloadCancelSource != null;
        
        protected override void OnSpawn()
        {
            CurrentAmmo = maxAmmo - maxLoadedAmmo;
            CurrentLoadedAmmo = maxLoadedAmmo;
        }

        public override void Hide()
        {
            base.Hide();
            shootingCancelSource?.Cancel();
            reloadCancelSource?.Cancel();
            shootingCancelSource = null;
            reloadCancelSource = null;
        }

        public override void StartShooting()
        {
            if (IsShooting) return;
            shootingCancelSource = new();
            
            _ = ShootRoutine(shootingCancelSource.Token);
        }

        public override void StopShooting()
        {
            if (!IsShooting) return;
            
            shootingCancelSource.Cancel();
            shootingCancelSource = null;
        }

        public override void Reload()
        {
            var requiredAmmo = maxLoadedAmmo - CurrentLoadedAmmo;
            if (IsReloading || requiredAmmo == 0 || CurrentAmmo == 0) return;
            reloadCancelSource = new();
            
            _ = ReloadRoutine(reloadCancelSource.Token);
        }

        private async Awaitable ReloadRoutine(CancellationToken cancelToken)
        {
            WeaponAudioSource.PlayOneShot(reloadSound);
            await Awaitable.WaitForSecondsAsync(reloadTimeSeconds, cancelToken);
            
            var requiredAmmo = maxLoadedAmmo - CurrentLoadedAmmo;

            var loadedAmmo = Mathf.Min(requiredAmmo, CurrentAmmo);
            CurrentAmmo -= loadedAmmo;
            Assert.IsFalse(CurrentAmmo < 0);
            
            CurrentLoadedAmmo += loadedAmmo;
            Assert.IsTrue(CurrentLoadedAmmo <= maxLoadedAmmo);
            reloadCancelSource = null;
        }

        private async Awaitable ShootRoutine(CancellationToken cancelToken)
        {
            while (true)
            {
                if (CurrentLoadedAmmo == 0)
                    WeaponAudioSource.PlayOneShot(emptySound);
                while (CurrentLoadedAmmo == 0 || IsReloading)
                    await Awaitable.NextFrameAsync(cancelToken);
                Shoot();
                await Awaitable.WaitForSecondsAsync(fireRateSeconds, cancelToken);
            }
        }

        private void Shoot()
        {
            CurrentLoadedAmmo--;
            WeaponAudioSource.PlayOneShot(shotSound);
            var hitsAmount = Physics.RaycastNonAlloc(ShootRay, hits, 1000);
            if (hitsAmount == 0) return;
            
            var hit = hits[hitsAmount - 1]; // last hit is the nearest hit
            if (hit.transform.gameObject.layer != EntityLayer) return;
            
            var entity = hit.transform.GetComponent<IEntity>();
            entity.ApplyDamage(damage);
        }
    }
}