using UnityEngine;
using UnityEngine.InputSystem;

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
        characterController.transform.Rotate(new Vector3() { y = delta.x });

        verticalLook = verticalLook + -delta.y;
        verticalLook = Mathf.Clamp(verticalLook, -maxVerticalLookAngle, maxVerticalLookAngle);

        var camEulers = cameraTransform.localEulerAngles;
        camEulers.x = verticalLook;
        cameraTransform.localEulerAngles = camEulers;
    }

    private void Update()
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
