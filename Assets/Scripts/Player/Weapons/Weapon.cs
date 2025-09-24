using UnityEngine;
using UnityEngine.Assertions;

namespace Player.Weapons
{
    public abstract class Weapon : ScriptableObject, IWeapon
    {
        protected static readonly LayerMask EntityLayer = 7;
        
        [SerializeField] private GameObject weaponPrefab;
        
        [SerializeField] protected AudioClip shotSound;
        [SerializeField] protected AudioClip emptySound;
        [SerializeField] protected AudioClip reloadSound;
    
        [Header("Stats")]
        [SerializeField] protected int maxLoadedAmmo;
        [SerializeField] protected int maxAmmo;
        [SerializeField] protected float fireRateSeconds;
        [SerializeField] protected float reloadTimeSeconds;
        [SerializeField] protected float damage;

        public int CurrentLoadedAmmo { get; protected set; }
        public int CurrentAmmo { get; protected set; }
        
        protected GameObject WeaponObject;
        protected AudioSource WeaponAudioSource;
        
        private Transform shootOrigin;
        
        protected Ray ShootRay => new Ray(shootOrigin.position, shootOrigin.forward);

        public void Spawn(Transform parent, Transform shootOrigin, AudioSource audioSource)
        {
            Assert.IsNull(WeaponObject);
            WeaponObject = Instantiate(weaponPrefab, parent);
            WeaponObject.SetActive(false);
            this.shootOrigin = shootOrigin;
            WeaponAudioSource = audioSource;
            OnSpawn();
        }
        
        protected virtual void OnSpawn() {}
        
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
    }
}
