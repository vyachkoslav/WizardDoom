using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    [Header("Door")]
    [SerializeField] private List<Door> _doorList = new List<Door>();
    
    [Header("Gauntlet")]
    [SerializeField] private bool _isGauntletRoom = true;
    [SerializeField] private List<GameObject> _enemyList = new List<GameObject>();

    private bool _isPlayerInRoom = false;
    public bool IsPlayerInRoom { get { return _isPlayerInRoom; } }

    private bool _isRoomCleared = false;
    public bool IsRoomCleared { get { return _isRoomCleared; } }

    public UnityEvent OnPlayerEnteredRoom = new();
    public UnityEvent OnPlayerLeftRoom = new();

    private void Start()
    {
        if (!_isGauntletRoom) { _isRoomCleared = true; }
    }

    // Check if player really has entered the room and fighting should start
    // Should prevent gauntlet softlocking caused by entering room slightly but backing out and closing door
    private void Update()
    {
        if (_isGauntletRoom && _isPlayerInRoom)
        {
            if (_enemyList.Count != 0 && PlayerEntity.Instance.Health > 0)
            {
                foreach (Door door in _doorList)
                {
                    if (door.IsDoorClosed)
                    {
                        DataManager.Instance.IsFighting = true;
                    }
                }
            }
            else { DataManager.Instance.IsFighting = false; }
        }
    }

    // Remove enemy from list and check if player is in combat
    public void RemoveEnemyFromList(GameObject enemy)
    {
        if (_enemyList.Contains(enemy)) 
        {
            _enemyList.Remove(enemy);
        }

        if (_enemyList.Count == 0)
        {
            DataManager.Instance.IsFighting = false;
            _isRoomCleared = true;
        }
    }

    private void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = true;
            OnPlayerEnteredRoom.Invoke();
        }
    }

    private void OnTriggerExit(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = false;
            OnPlayerLeftRoom.Invoke();
        }
    }
}
