using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

// Abstract class for pickups like hp, ammo
public abstract class Pickup : MonoBehaviour
{
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _model;

    [Header("Stats")]
    [SerializeField] protected Vector3 _rotationAxis;
    [SerializeField] protected float _rotationSpeed;
    [SerializeField] protected float _respawnTimeInSeconds;

    [Header("ID")]
    [SerializeField] protected string _id;
    public string ID { get { return _id; } }

    private List<Pickup> _upgradeList;

    protected virtual void Start()
    {
        _upgradeList = DataManager.Instance.UpgradeList;
        foreach (Pickup upgrade in _upgradeList)
        {
            if (upgrade.ID == _id)
            {
                this.gameObject.SetActive(false); 
            }
        }
    }

    // Handle pickup rotation
    protected virtual void FixedUpdate() 
    {
        transform.RotateAround(transform.position, _rotationAxis, _rotationSpeed * Time.deltaTime);
    }

    // Handle collision with player, disappear pickup and respawn if needed
    protected virtual void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        if (target == PlayerEntity.Instance.gameObject)
        {
            PickupEffect();
            _model.SetActive(false);
            _collider.enabled = false;
            if (_respawnTimeInSeconds > 0) { StartCoroutine(Respawn()); }
            PlayerEntity.Instance.gameObject.GetComponent<InteractionController>().DisplayPickupText(this.ToString());
        }
    }

    // Implement pickup functionality in extending classes
    protected abstract void PickupEffect();

    // Handle pickup respawning
    protected virtual IEnumerator Respawn()
    {
        yield return new WaitForSeconds(_respawnTimeInSeconds);
        _model.SetActive(true);
        _collider.enabled = true;
    }
}
