using UnityEngine;

namespace Player
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [SerializeField] private CharacterControls _playerControls;
        [SerializeField] private float _stepIntervalInSeconds;

        private float _stepTimer;
        private float _timerInSeconds;

        private void Start()
        {
            _timerInSeconds = _stepIntervalInSeconds;
        }

        private void Update()
        {
            // For consistent audiovisuals, only start footsteps when player is at max movement speed
            if (_playerControls.CurrentSpeed == _playerControls.MaxSpeed)
            {
                if (_timerInSeconds >= _stepIntervalInSeconds)
                {
                    SoundManager.Instance.PlaySound2D("PlayerFootstep"); //TODO? maybe make a better sfx
                    _stepTimer = 0;
                }

                _stepTimer += Time.deltaTime;
                _timerInSeconds = _stepTimer % 60; // Convert frame timer to seconds
            }
            else
            {
                _stepTimer = 0;
                _timerInSeconds = _stepIntervalInSeconds;
            }
        }
    }
}
