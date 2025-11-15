using System.Collections;
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
        GameObject target = _.gameObject;

        if (target.layer == 7) //TODO fix magic number later
        {
            // Ignore targets that are not enemy, such as projectiles
            if (!target.GetComponent<BaseEnemyAI>())
            {
                return;
            }

            Debug.Log("Frozen " + target);
            target.GetComponent<EnemyTimefreeze>().ApplyTimefreeze(_durationInSeconds);
        }
        Destroy(this.gameObject);
    }

    protected override void Update() { return; }
}
