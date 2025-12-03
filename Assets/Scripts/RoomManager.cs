using System.Collections.Generic;
using Player;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    
    [Header("Gauntlet")]
    [SerializeField] private List<GameObject> _enemyList = new List<GameObject>();

    private bool _isPlayerInRoom = false;
    public bool IsPlayerInRoom { get { return _isPlayerInRoom; } }

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
        }
    }

    private void OnTriggerExit(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            _isPlayerInRoom = false;
        }
    }
}
