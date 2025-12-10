using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private RoomManager _myRoom;

    private void Start()
    {
        if (_myRoom == null)
        {
            DataManager.Instance.CheckPoint = transform.position;
        }
    }

    public void SetCheckpoint()
    {
        if (_myRoom.IsRoomCleared)
        {
            DataManager.Instance.CheckPoint = transform.position;
        }
    }
}
