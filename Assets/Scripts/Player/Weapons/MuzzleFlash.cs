using System.Collections;
using UnityEngine;

namespace Player.Weapons
{
    public class MuzzleFlash : MonoBehaviour
    {
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private float flashDurationInSeconds;

        private void Start()
        {
            muzzleFlash.SetActive(false);
        }

        public void ActivateMuzzleFlash()
        {
            StartCoroutine(MuzzleFlashCoroutine());
        }

        private IEnumerator MuzzleFlashCoroutine()
        {
            muzzleFlash.transform.Rotate(0, 0, 45);
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(flashDurationInSeconds);
            muzzleFlash.SetActive(false);
        }
    }
}