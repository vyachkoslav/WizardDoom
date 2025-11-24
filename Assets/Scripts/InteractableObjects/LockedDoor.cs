using System.Collections;
using Player;
using UnityEngine;

public class LockedDoor : Interactable
{
    [SerializeField] private Key _key;
    [SerializeField] private HingeJoint _hingeJoint;
    [SerializeField] private float _doorOpenTimeInSeconds = 1f;

    public override void Interact()
    {
        // Check if assigned key is already obtained
        if (DataManager.Instance.KeyList.Contains(_key))
        {
            StartCoroutine(OpenDoor());
            base.Interact();
        }
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