using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractionControls : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private Collider _playerCollider;

        public static int InteractionObjectLayer = 10; // InteractableObject layermask

        private Interactable _interactable;
        private bool _canInteract = false;

        private void OnEnable()
        {
            interactAction.action.performed += StartInteraction;
        }

        private void OnDisable()
        {
            interactAction.action.performed -= StartInteraction;
        }

        private void StartInteraction(InputAction.CallbackContext context)
        {
            if (_canInteract)
            {
                _interactable.Interact();
            }
        }

        private void OnTriggerEnter(Collider _)
        {
            var target = _.gameObject;

            if (target.layer == InteractionObjectLayer)
            {
                _interactable = target.GetComponent<Interactable>(); // Assign interactable object
                _canInteract = _interactable.CanInteract; // Check if able to interact with object
            }
            else { _canInteract = false; }
        }
    }
}