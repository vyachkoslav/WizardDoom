using System.Collections;
using Player;
using UnityEngine;

public class Door : MonoBehaviour 
{
    [SerializeField] private Key _key;
    [SerializeField] private HingeJoint _hingeJoint;

    private void OnTriggerEnter(Collider _) 
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            if (DataManager.Instance.KeyList.Contains(_key))
            {
                StartCoroutine(OpenDoor());
            }
        }
    }

    // Activates Hinge Joint component, parameters configured in the editor
    private IEnumerator OpenDoor()
    {
        _hingeJoint.useSpring = true;
        // SoundManager.Instance.PlaySound3D("DoorOpen", transform.position);
        yield return new WaitForSeconds(1);
        _hingeJoint.useSpring = false;
    }
}