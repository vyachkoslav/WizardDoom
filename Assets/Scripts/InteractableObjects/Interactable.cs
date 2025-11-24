using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
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
}