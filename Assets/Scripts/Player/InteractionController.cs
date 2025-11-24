using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractionControls : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private Collider _playerCollider;

        [Header("UI")]
        [SerializeField] private TMP_Text _interactionText;

        public static int InteractionObjectLayer = 10; // InteractableObject layermask

        private Interactable _interactable;
        private bool _canInteract = false;

        private void Start()
        {
            _interactionText.enabled = false;
        }

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

        // To enable interaction and display text on HUD
        private void OnTriggerEnter(Collider _)
        {
            var target = _.gameObject;

            if (target.layer == InteractionObjectLayer)
            {
                _interactable = target.GetComponent<Interactable>(); // Assign interactable object

                if (_interactable.CanInteract)
                {
                    _canInteract = _interactable.CanInteract; // Check if able to interact with object
                    _interactionText.enabled = true;
                    _interactionText.text = _interactable.DisplayText();
                }
                else { _canInteract = false; }
            }
            else { _canInteract = false; }
        }

        // To disable interaction and hide text on HUD
        private void OnTriggerExit(Collider _)
        {
            var target = _.gameObject;

            if (target.layer == InteractionObjectLayer)
            {
                _interactable = target.GetComponent<Interactable>();
                _canInteract = false;
                _interactionText.enabled = false;
            }
            else { _canInteract = false; }
        }
    }
}