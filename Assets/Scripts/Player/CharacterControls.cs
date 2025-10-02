using NUnit.Framework;
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
        [SerializeField] private float speed;

        private Vector3 direction;
        private float verticalLook;

        //Is used when pausing the game
        private bool isPaused;
        //The game now checks if the pause menu is enabled and dissallows camera movement if it is
        public void AddCameraDelta(Vector2 delta)
        { 
            if (!isPaused)
            {
                characterController.transform.Rotate(new Vector3() { y = delta.x });

                verticalLook = verticalLook + -delta.y;
                verticalLook = Mathf.Clamp(verticalLook, -maxVerticalLookAngle, maxVerticalLookAngle);

                var camEulers = cameraTransform.localEulerAngles;
                camEulers.x = verticalLook;
                cameraTransform.localEulerAngles = camEulers;
            }

        }

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
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


        //Switches the pause state(from PauseMenu script)
        public void TogglePause()
        {
            isPaused = !isPaused;
        }

        //Shows the cursor it the game is paused and hides it if the game is not paused
        private void ToggleCursor()
        {
            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }

        //Checks if the game is paused or not and allows movement
        private void Movement()
        {
            if (!isPaused)
            {
                var forward = characterController.transform.forward;
                var right = characterController.transform.right;
                var moveAmount = Time.deltaTime * speed;
                var movementY = (direction.y * moveAmount) * forward;
                var movementX = (direction.x * moveAmount) * right;
                var movement = movementX + movementY;
                characterController.Move(movement);
            }
        }

        private void Update()
        {
            Movement();
            ToggleCursor();
        }
    }
}
