using System.Collections;
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

    // Handle pickup rotation
    protected virtual void FixedUpdate() 
    {
        transform.RotateAround(transform.position, _rotationAxis, _rotationSpeed * Time.deltaTime);
    }

    // Handle pickup functionality
    protected abstract void OnTriggerEnter(Collider _);

    // Handle pickup respawning
    protected virtual IEnumerator Respawn()
    {
        _model.SetActive(false);
        _collider.enabled = false;
        
        if (_respawnTimeInSeconds != 0)
        {
            yield return new WaitForSeconds(_respawnTimeInSeconds);
            _model.SetActive(true);
            _collider.enabled = true;
        }
        else { yield break;}
    }
}
