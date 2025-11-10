using UnityEngine;

namespace Player
{
    public class CameraFX : MonoBehaviour
    {
        private enum SpeedState
        {
            Zero,
            Moving,
            Stopping
        }
        
        [SerializeField] private CharacterControls charCtrl;
        [SerializeField] private new Camera camera;

        [SerializeField] private float cameraShakeSpeed;
        [SerializeField] private Vector2 cameraShakeMagnitude;
        
        [SerializeField] private float fovDeltaAtMaxSpeed;
        [SerializeField] private float diminishFovAfterSeconds;
        [SerializeField] private float diminishLerpRateMultiplier;

        private float timeWaited;
        private float defaultFov;
        private Vector3 defaultLocalPosition;
        private SpeedState state;

        private void Awake()
        {
            defaultFov = camera.fieldOfView;
            defaultLocalPosition = camera.transform.localPosition;
        }

        private void Update()
        {
            if (charCtrl.CurrentSpeed == 0 && state == SpeedState.Moving)
            {
                _ = Stopping();
            }
            else if (charCtrl.CurrentSpeed != 0 && state != SpeedState.Moving)
            {
                _ = Moving();
            }
        }

        private async Awaitable Moving()
        {
            state = SpeedState.Moving;
            while (state == SpeedState.Moving)
            {
                camera.fieldOfView += charCtrl.Accelation * Time.deltaTime;
                if (camera.fieldOfView > defaultFov + fovDeltaAtMaxSpeed)
                {
                    camera.fieldOfView = defaultFov + fovDeltaAtMaxSpeed;
                    break;
                }
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }

            while (state == SpeedState.Moving)
            {
                var timeVal = Time.realtimeSinceStartup * cameraShakeSpeed;
                var posX = defaultLocalPosition.x + Mathf.Sin(timeVal) * cameraShakeMagnitude.x;
                var posY = defaultLocalPosition.y + Mathf.Cos(timeVal * 2) * cameraShakeMagnitude.y;
                camera.transform.localPosition = new Vector3(posX, posY, camera.transform.localPosition.z);
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }
            camera.transform.localPosition = defaultLocalPosition;
        }

        private async Awaitable Stopping()
        {
            state = SpeedState.Stopping;
            timeWaited = 0;
            while (state == SpeedState.Stopping && timeWaited < diminishFovAfterSeconds)
            {
                await Awaitable.NextFrameAsync(destroyCancellationToken);
                timeWaited += Time.deltaTime;
            }

            if (state != SpeedState.Stopping)
                return;
            
            while (state == SpeedState.Stopping)
            {
                camera.fieldOfView -= charCtrl.Accelation * diminishLerpRateMultiplier * Time.deltaTime;
                if (camera.fieldOfView < defaultFov)
                {
                    camera.fieldOfView = defaultFov;
                    state = SpeedState.Zero;
                    break;
                }
                await Awaitable.NextFrameAsync(destroyCancellationToken);
            }
        }
    }
}