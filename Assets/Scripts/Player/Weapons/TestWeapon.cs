using System;
using System.Collections.Generic;
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
        [SerializeField] private float recoilSmoothTime = 0.05f;
        [SerializeField] private List<Vector2> recoilPattern = new();
        
        private CancellationTokenSource cancelSource;

        private int continuosShotCount = 0;
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

        protected override void OnDestroy()
        {
            Hide();
            cancelSource?.Cancel();
        }

        public override void Hide()
        {
            base.Hide();
            StopShooting();
            isReloading = false;
        }

        public override void StartShooting()
        {
            isShooting = true;
        }

        public override void StopShooting()
        {
            isShooting = false;
            continuosShotCount = 0;
        }

        public override void Reload()
        {
            StopShooting();
            isReloading = true;
        }

        private async Awaitable ReloadRoutine(CancellationToken cancelToken)
        {
            try
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
                    SoundManager.Instance.PlaySound2D("WeaponReload");
                    WeaponObject.GetComponent<Animator>().SetTrigger("TrEmptyReload");
                    var startTime = Time.time;
                    while (isReloading)
                    {
                        if (Time.time - startTime >= reloadTimeSeconds) break;
                        await Awaitable.NextFrameAsync(cancelToken);
                    }

                    if (!isReloading)
                    {
                        //WeaponAudioSource.Stop();
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
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private async Awaitable ShootRoutine(CancellationToken cancelToken)
        {
            try
            {
                var lastShot = float.MinValue;
                while (WeaponObject != null)
                {
                    await Awaitable.NextFrameAsync(cancelToken);
                    if (!isShooting) continue;

                    if (CurrentLoadedAmmo == 0)
                        SoundManager.Instance.PlaySound2D("WeaponEmpty");
                    if (CurrentLoadedAmmo == 0 || isReloading)
                    {
                        StopShooting();
                        continue;
                    }

                    while (isShooting)
                    {
                        if (Time.time - lastShot >= fireRateSeconds) break;
                        if (!isAutomatic)
                        {
                            StopShooting();
                            break;
                        }

                        await Awaitable.NextFrameAsync(cancelToken);
                    }

                    if (!isShooting) continue;

                    lastShot = Time.time;
                    Shoot();
                    if (continuosShotCount < recoilPattern.Count)
                        RecoilController.AddRecoil(recoilPattern[continuosShotCount++], recoilSmoothTime);
                    if (!isAutomatic)
                        StopShooting();
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void Shoot()
        {
            CurrentLoadedAmmo--;
            WeaponObject.GetComponent<Animator>().SetTrigger("TrShoot");
            PlayerEntity.Instance.gameObject.GetComponent<MuzzleFlash>().ActivateMuzzleFlash();

            //Soundmanagment for different guns
            if (!isAutomatic)
            {
                SoundManager.Instance.PlaySound2D("ShotgunFire");
            }
            else{SoundManager.Instance.PlaySound2D("MachinegunFire");}
            var didHit = Physics.Raycast(ShootRay, out var hit, 1000);
            if (!didHit || !hit.transform.TryGetComponent<IEntity>(out var entity)) return;

            entity.ApplyDamage(damage);
        }
    }
}