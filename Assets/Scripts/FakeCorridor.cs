using Player;
using UnityEngine;

public class FakeCorridor : MonoBehaviour
{
    // [SerializeField] private GameObject _door;

    private void OnTriggerExit(Collider _)
    {
        var target = _.gameObject;

        if (target.layer == PlayerEntity.Instance.gameObject.layer)
        {
            this.gameObject.SetActive(false);
        }
    }
}
