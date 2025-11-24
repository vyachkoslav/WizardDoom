using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected InputActionReference interactAction;

    protected bool _canInteract;
    public bool CanInteract { get { return _canInteract; } }

    protected void Start()
    {
        _canInteract = true; // Can interact by default
    }   
    
    // Implement interaction here
    public virtual void Interact()
    {
        _canInteract = false; // Use base.Interact in the end to disable interaction
    }

    public abstract string DisplayText(); // Display text on player HUD about the interaction
}