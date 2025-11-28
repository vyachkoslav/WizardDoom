using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected InputActionReference interactAction;

    protected bool _canInteract = true;
    public bool CanInteract { get { return _canInteract; } }
    
    // Implement interaction here, remember to set _canInteract to false after interaction
    public abstract void Interact();

    public abstract string DisplayText(); // Display text on player HUD about the interaction
}