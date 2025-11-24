using System.Collections;
using UnityEngine;

public class UnlockedDoor : Interactable
{
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private float _doorOpenTimeInSeconds = 1f;

    public override void Interact()
    {
        StartCoroutine(OpenDoor());
        base.Interact(); // _canInteract = false;
    }

    // Activates Hinge Joint component, parameters should be configured in the editor
    private IEnumerator OpenDoor()
    {
        _hingeJoint.useSpring = true;
        // SoundManager.Instance.PlaySound3D("DoorOpen", transform.position);
        yield return new WaitForSeconds(_doorOpenTimeInSeconds);
        _hingeJoint.useSpring = false;
    }
}
