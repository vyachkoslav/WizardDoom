using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Collider _collider;

    [Header("Door")]
    // [SerializeField] private List<Door> _doorList = new List<Door>();
    [SerializeField] private Door _door;
    
    [Header("Gauntlet")]
    [SerializeField] private List<GameObject> _enemyList = new List<GameObject>();

    private bool _isPlayerInRoom = false;
    public bool IsPlayerInRoom { get { return _isPlayerInRoom; } }

    public UnityEvent OnPlayerEnteredRoom = new();
    public UnityEvent OnPlayerLeftRoom = new();

    // Check if player really has entered the room and fighting should start
    // Should prevent gauntlet softlocking caused by entering room slightly but backing out and closing door
    private void Update()
    {
        if (_isPlayerInRoom)
        {
            if (_door.IsDoorClosed && _enemyList.Count != 0)
            {
                DataManager.Instance.IsFighting = true;
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
        }
    }

    private void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = true;
            OnPlayerEnteredRoom.Invoke();
            Debug.Log(_isPlayerInRoom);
        }
    }

    private void OnTriggerExit(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = false;
            OnPlayerLeftRoom.Invoke();
            Debug.Log(_isPlayerInRoom);
        }
    }
}
