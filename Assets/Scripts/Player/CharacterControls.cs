using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class CharacterControls : MonoBehaviour

    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Transform cameraTransform;

        [Header("Input")]
        [SerializeField] private InputActionReference movementAction;
        [SerializeField] private InputActionReference lookAction;

        [Header("Settings")]
        [SerializeField] private float maxVerticalLookAngle;
        [SerializeField] private float lookSensitivity;
        [SerializeField] private float acceleration;
        [SerializeField] private float speed;
        
        private float currentSpeed;
        private Vector3 direction;
        private float verticalLook;
        
        public float CurrentSpeed => currentSpeed;
        public float MaxSpeed => speed;
        public float Accelation => acceleration;

        public void AddCameraDelta(Vector2 delta)
        {
            characterController.transform.Rotate(new Vector3() { y = delta.x });

            verticalLook = verticalLook + -delta.y;
            verticalLook = Mathf.Clamp(verticalLook, -maxVerticalLookAngle, maxVerticalLookAngle);
            var camEulers = cameraTransform.localEulerAngles;
            camEulers.x = verticalLook;
            cameraTransform.localEulerAngles = camEulers;
        }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            movementAction.action.performed += OnMove;
            movementAction.action.canceled += OnMove;
            lookAction.action.performed += OnLook;
        }
        private void OnDisable()
        {
            movementAction.action.performed -= OnMove;
            movementAction.action.canceled -= OnMove;
            lookAction.action.performed -= OnLook;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            direction = ctx.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext ctx)

        {
            var delta = ctx.ReadValue<Vector2>();
            delta *= lookSensitivity;

            AddCameraDelta(delta);

        }

        private void Update()
        {
            if (direction == Vector3.zero)
                currentSpeed = 0;
            else
                currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.deltaTime);
            
            var forward = characterController.transform.forward;
            var right = characterController.transform.right;
            var movementY = (direction.y * currentSpeed) * forward;
            var movementX = (direction.x * currentSpeed) * right;
            var movement = movementX + movementY;
            characterController.SimpleMove(movement);
        }

    }

}
