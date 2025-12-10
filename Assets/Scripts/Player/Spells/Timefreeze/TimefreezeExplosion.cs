using Player;
using UnityEngine;

public class TimefreezeExplosion : Explosion
{
    protected override void Awake()
    {
        base.Awake();
        SoundManager.Instance.PlaySound3D("Timefreeze", transform.position);
    }

    public void Spawn(float freezeDuration, float radius)
    {
        _durationInSeconds = freezeDuration;

        myCollider.radius = radius;
    }

    // Disable enemy AI for targets inside explosion
    protected override void OnTriggerEnter(Collider _)
    {
        var target = _.gameObject;

        // Ignore player and other projectiles
        if (target != PlayerEntity.Instance.gameObject && !target.CompareTag("Projectile")) 
        {
            target.GetComponent<EnemyTimefreeze>()?.ApplyTimefreeze(_durationInSeconds);
            Destroy(this.gameObject);
        }
        
    }

    protected override void Update() { return; }
}
