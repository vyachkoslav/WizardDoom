using Player;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    private bool _isPlayerInRoom = false;
    public bool IsPlayerInRoom { get { return _isPlayerInRoom; } }

    private void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;
        if (target = PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = true;
        }

        else { _isPlayerInRoom = false; }
    }

    private void OnTriggerExit(Collider _)
    {
        var target = _.gameObject;

        if (target = PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = false;
        }
    }
}
