using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class Door : Interactable
{
    [SerializeField] protected Collider _doorModelCollider;
    [SerializeField] protected HingeJoint _hingeJoint;
    [SerializeField] protected Rigidbody _rigidBody;
    [SerializeField] protected float _doorBufferInSeconds = 0.5f;

    protected JointLimits _limits;
    protected JointSpring _spring;
    protected float _limitMin;
    protected float _limitMax;
    protected float _springTarget;
    protected float _springForce;
    protected float _springDamper;

    // RoomManager uses to determine lockdown
    protected bool _isDoorClosed = false;
    public bool IsDoorClosed { get { return _isDoorClosed; } }

    protected virtual void Start()
    {
        _limitMin = _hingeJoint.limits.min;
        _limitMax = _hingeJoint.limits.max;
        _springTarget = _hingeJoint.spring.targetPosition;
        _springForce = _hingeJoint.spring.spring;
        _springDamper = _hingeJoint.spring.damper;

        _rigidBody.isKinematic = true;
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
        _doorModelCollider.enabled = false;
        _rigidBody.isKinematic = false;
        _hingeJoint.useSpring = true;
        SoundManager.Instance.PlaySound3D("Door", transform.position);
        yield return new WaitForSeconds(_doorBufferInSeconds);
        _hingeJoint.useSpring = false;
        _doorModelCollider.enabled = true;

        _canInteract = false;
        _isDoorClosed = false;
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
        _doorModelCollider.enabled = false;
        _hingeJoint.useSpring = true;
        SoundManager.Instance.PlaySound3D("Door", transform.position);
        yield return new WaitForSeconds(_doorBufferInSeconds);
        _hingeJoint.useSpring = false;
        _doorModelCollider.enabled = true;
        
        _canInteract = true;
        _isDoorClosed = true;
        _rigidBody.isKinematic = true;
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