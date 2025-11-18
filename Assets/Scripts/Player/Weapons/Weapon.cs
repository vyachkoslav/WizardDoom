using UnityEngine;
using UnityEngine.Assertions;
using Utils;

namespace Player.Weapons
{
    public abstract class Weapon : ScriptableObject, IWeapon
    {
        protected static readonly LayerMask EntityLayer = 7;
        
        [SerializeField] private GameObject weaponPrefab;
        

    
        [Header("Stats")]
        [SerializeField] protected int maxLoadedAmmo;
        [SerializeField] protected int maxAmmo;
        [SerializeField] protected float fireRateSeconds;
        [SerializeField] protected float reloadTimeSeconds;
        [SerializeField] protected float damage;

        public int CurrentLoadedAmmo { get; protected set; }
        public int CurrentAmmo { get; protected set; }
        
        protected GameObject WeaponObject;

        protected RecoilController RecoilController;
        
        protected Transform ShootOrigin;
        
        protected Ray ShootRay => new Ray(ShootOrigin.position, ShootOrigin.forward);

        public void Spawn(Transform parent, Transform shootOrigin, RecoilController recoilController)
        {
            Assert.IsNull(WeaponObject);
            WeaponObject = Instantiate(weaponPrefab, parent);
            WeaponObject.AddComponent<OnDestroyEventProvider>().OnDestroyed.AddListener(OnDestroy);
            WeaponObject.SetActive(false);
            ShootOrigin = shootOrigin;
            RecoilController = recoilController;
            OnSpawn();
        }
        
        protected virtual void OnSpawn() {}
        protected virtual void OnDestroy() {}
        
        // TODO: animation and sound
        public virtual void Show()
        {
            if (WeaponObject.activeSelf) return;
            WeaponObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (!WeaponObject.activeSelf) return;
            StopShooting();
            WeaponObject.SetActive(false);
        }

        public abstract void StartShooting();
        public abstract void StopShooting();
        public abstract void Reload();

        // Don't let player get more ammo than max
        public virtual void AddAmmo(int ammoToAdd)
        {
            CurrentAmmo += ammoToAdd;
            if (CurrentAmmo > maxAmmo) 
            { 
                CurrentAmmo = maxAmmo; 
            }
        }
    }
}
