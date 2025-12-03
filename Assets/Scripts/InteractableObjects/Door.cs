using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : Interactable
{
    [SerializeField] protected Collider _doorCollider;
    [SerializeField] protected HingeJoint _hingeJoint;
    [SerializeField] protected float _doorBufferInSeconds = 1f;

    protected JointLimits _limits;
    protected JointSpring _spring;
    protected float _limitMin;
    protected float _limitMax;
    protected float _springTarget;
    protected float _springForce;
    protected float _springDamper;

    protected virtual void Start()
    {
        _limitMin = _hingeJoint.limits.min;
        _limitMax = _hingeJoint.limits.max;
        _springTarget = _hingeJoint.spring.targetPosition;
        _springForce = _hingeJoint.spring.spring;
        _springDamper = _hingeJoint.spring.damper;
    }

    public override void Interact()
    {
        if (!DataManager.Instance.IsFighting)
        {
            StartCoroutine(OpenDoor());
        }
    }

    // For opening door
    protected virtual IEnumerator OpenDoor()
    {
        SetSpring(_limitMax);
        _doorCollider.enabled = false;
        _hingeJoint.useSpring = true;
        // SoundManager.Instance.PlaySound3D("Door", transform.position);
        yield return new WaitForSeconds(_doorBufferInSeconds);
        _hingeJoint.useSpring = false;
        _doorCollider.enabled = true;

        _canInteract = false;
    }

    // Close door behind player after leaving trigger collider
    protected virtual void OnTriggerExit(Collider _)
    {
        var target = _.gameObject;

        if (target.layer == PlayerEntity.Instance.gameObject.layer && !_canInteract && !_hingeJoint.useSpring)
        {
            StartCoroutine(CloseDoor());
        }
    }

    // For closing door
    protected virtual IEnumerator CloseDoor()
    {
        SetSpring(_limitMin);
        _doorCollider.enabled = false;
        _hingeJoint.useSpring = true;
        // SoundManager.Instance.PlaySound3D("Door", transform.position);
        yield return new WaitForSeconds(_doorBufferInSeconds);
        _hingeJoint.useSpring = false;
        _doorCollider.enabled = true;
        
        _canInteract = true;
    }

    // Reverse hinge joint movement direction
    protected void SetSpring(float value)
    {
        _spring.targetPosition = value;
        _spring.spring = _springForce;
        _spring.damper = _springDamper;
        _hingeJoint.spring = _spring;
    }

    // Display text on HUD
    public override string DisplayText()
    {
        if (DataManager.Instance.IsFighting)
        {
            return "Defeat all enemies!";
        }
        else
        {
            return "Press '" + interactAction.action.GetBindingDisplayString() + "' to open door.";
        }
    }
}