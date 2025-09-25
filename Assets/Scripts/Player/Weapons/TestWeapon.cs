using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player.Weapons
{
    [CreateAssetMenu(fileName = "Test Weapon", menuName = "Weapons/TestWeapon")]
    public class TestWeapon : Weapon
    {
        private readonly RaycastHit[] hits = new RaycastHit[2];

        [SerializeField] private bool isAutomatic;
        
        private CancellationTokenSource cancelSource;

        private bool isShooting = false;
        private bool isReloading = false;
        
        protected override void OnSpawn()
        {
            CurrentAmmo = maxAmmo - maxLoadedAmmo;
            CurrentLoadedAmmo = maxLoadedAmmo;
            cancelSource = new();
            _ = ShootRoutine(cancelSource.Token);
            _ = ReloadRoutine(cancelSource.Token);
        }

        public override void Hide()
        {
            base.Hide();
            isShooting = false;
            isReloading = false;
        }

        public override void StartShooting()
        {
            isShooting = true;
        }

        public override void StopShooting()
        {
            isShooting = false;
        }

        public override void Reload()
        {
            isReloading = true;
        }

        private async Awaitable ReloadRoutine(CancellationToken cancelToken)
        {
            while (WeaponObject != null)
            {
                await Awaitable.NextFrameAsync(cancelToken);
                if (!isReloading) continue;
                
                if (maxLoadedAmmo == CurrentLoadedAmmo || CurrentAmmo == 0)
                {
                    isReloading = false;
                    continue;
                }

                CurrentAmmo += CurrentLoadedAmmo;
                CurrentLoadedAmmo = 0;
                WeaponAudioSource.PlayOneShot(reloadSound);
                var startTime = Time.time;
                while (isReloading)
                {
                    if (Time.time - startTime >= reloadTimeSeconds) break;
                    await Awaitable.NextFrameAsync(cancelToken);
                }

                if (!isReloading)
                {
                    WeaponAudioSource.Stop();
                    continue;
                }

                var loadedAmmo = Mathf.Min(maxLoadedAmmo, CurrentAmmo);
                CurrentAmmo -= loadedAmmo;
                Assert.IsFalse(CurrentAmmo < 0);

                CurrentLoadedAmmo += loadedAmmo;
                Assert.IsTrue(CurrentLoadedAmmo <= maxLoadedAmmo);
                isReloading = false;
            }
        }

        private async Awaitable ShootRoutine(CancellationToken cancelToken)
        {
            var lastShot = float.MinValue;
            while (WeaponObject != null)
            {
                await Awaitable.NextFrameAsync(cancelToken);
                if (!isShooting) continue;

                if (CurrentLoadedAmmo == 0)
                    WeaponAudioSource.PlayOneShot(emptySound);
                if (CurrentLoadedAmmo == 0 || isReloading)
                {
                    isShooting = false;
                    continue;
                }
                while (isShooting)
                {
                    if (Time.time - lastShot >= fireRateSeconds) break;
                    if (!isAutomatic)
                    {
                        isShooting = false;
                        break;
                    }
                    await Awaitable.NextFrameAsync(cancelToken);
                }
                if (!isShooting) continue;
                
                lastShot = Time.time;
                Shoot();
                if (!isAutomatic)
                    isShooting = false;
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