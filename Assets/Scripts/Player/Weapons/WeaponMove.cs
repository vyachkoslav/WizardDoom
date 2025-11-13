using UnityEngine;

namespace Player.Weapons
{
    public class WeaponMove : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private CharacterControls _characterControls;
        private bool _isMoving = false;

        private void Start()
        {
            _characterControls = FindAnyObjectByType<CharacterControls>();
        }

        private void Update()
        {
            if (_characterControls.CurrentSpeed == _characterControls.MaxSpeed)
            {
                _isMoving = true;
            }
            else { _isMoving = false; }

            _animator.SetBool("IsMoving", _isMoving);
        }
    }
}
